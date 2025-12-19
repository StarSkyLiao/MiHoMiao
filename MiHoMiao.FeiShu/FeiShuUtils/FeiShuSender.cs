using System.Net;
using System.Text;
using System.Text.Json;

namespace MiHoMiao.FeiShu.FeiShuUtils;

public class FeiShuSender(string appId, string appSecret)
{
    private readonly HttpClient m_HttpClient = new HttpClient();
    private string? m_AccessToken;
    private DateTime m_TokenExpiry;

    /// <summary>
    /// 获取访问令牌
    /// </summary>
    private async Task<string> GetAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(m_AccessToken) && m_TokenExpiry > DateTime.Now) return m_AccessToken;
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 正在获取新的访问令牌...");
        var requestData = new
        {
            app_id = appId,
            app_secret = appSecret
        };
        
        const string Address = "https://fsopen.bytedance.net/open-apis/auth/v3/tenant_access_token/internal/";
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 发送请求到: {Address}");
        string stringContent = JsonSerializer.Serialize(requestData);
        StringContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 请求内容: {stringContent})");
        HttpResponseMessage response = await m_HttpClient.PostAsync(Address, content);
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 请求access_token的结果: {response.StatusCode}");
        
        string result = await response.Content.ReadAsStringAsync();
        using JsonDocument jsonDoc = JsonDocument.Parse(result);
        JsonElement root = jsonDoc.RootElement;

        if (root.GetProperty("code").GetInt32() != 0) throw new Exception($"[{DateTime.Now:HH:mm:ss}] 获取访问令牌失败: {result}");

        m_AccessToken = root.GetProperty("tenant_access_token").GetString();
        int expire = root.GetProperty("expire").GetInt32();
        m_TokenExpiry = DateTime.Now.AddSeconds(expire - 60);

        return m_AccessToken!;
    }

    /// <summary>
    /// 根据邮箱获取用户ID
    /// </summary>
    private async Task<string> GetUserIdByEmailAsync(string email)
    {
        string accessToken = await GetAccessTokenAsync();
            
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
            $"https://open.feishu.cn/open-apis/contact/v3/users/find_by_email?email={email}")
        {
            Headers = { { "Authorization", $"Bearer {accessToken}" } }
        };

        HttpResponseMessage response = await m_HttpClient.SendAsync(request);
        string result = await response.Content.ReadAsStringAsync();

        using JsonDocument jsonDoc = JsonDocument.Parse(result);
        JsonElement root = jsonDoc.RootElement;

        if (root.GetProperty("code").GetInt32() != 0) throw new Exception($"获取用户ID失败: {result}");

        return root.GetProperty("data").GetProperty("user").GetProperty("user_id").GetString();
    }

    /// <summary>
    /// 发送消息（公开方法）
    /// </summary>
    public async Task SendMessage(string title, string text, string emailAddress)
    {
        MessageContent content = new MessageContent(text);
        await SendRichText(title, [[content]], emailAddress);
    }

    /// <summary>
    /// 发送富文本消息
    /// </summary>
    public async Task SendRichText(string title, MessageContent[][] content, string emailAddress)
    {
        try
        {
            // string userId = await GetUserIdByEmailAsync(emailAddress);
            string accessToken = await GetAccessTokenAsync();

            string requestData = WrapRichMsg(title, content, emailAddress);

            StringContent stringContent = new StringContent(requestData, Encoding.UTF8, "application/json");
            
            const string Address = "https://fsopen.bytedance.net/open-apis/message/v4/send/";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 发送请求到: {Address}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 请求内容: {requestData})");
            
            // 创建 HttpRequestMessage
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Address);
            request.Content = stringContent;
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
            request.Headers.TryAddWithoutValidation("Content-Type", $"application/json");
            
            HttpResponseMessage response = await m_HttpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            
            if (response.StatusCode is not HttpStatusCode.OK) throw new Exception(result);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 操作结果代码: {response.StatusCode})");
            
            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            if (root.GetProperty("code").GetInt32() != 0) throw new Exception(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 发送富文本消息失败:");
            Console.WriteLine($"{ex.Message}");
        }
    }

    /// <summary>
    /// 包装富文本消息
    /// </summary>
    private static string WrapRichMsg(string title, MessageContent[][] content, string receiveId)
    {
        var messageData = new
        {
            msg_type = "post",
            content = new { post = new { zh_cn = new { title, content } } }
        };

        // 判断是邮箱还是群聊ID
        return (receiveId.Contains('@'))
            ? JsonSerializer.Serialize(new
            {
                email = receiveId,
                msg_type = "post",
                messageData.content
            })
            : JsonSerializer.Serialize(new
            {
                chat_id = receiveId,
                msg_type = "post",
                messageData.content
            });
    }

    /// <summary>
    /// 包装富文本消息（添加样式和格式）
    /// </summary>
    public async Task<bool> WrapRichText(string title, string text, string emailAddress)
    {
        try
        {
            string userId = await GetUserIdByEmailAsync(emailAddress);
            string accessToken = await GetAccessTokenAsync();

            string wrappedContent = $"**{title}**\n\n{text}";
                
            var requestBody = new
            {
                receive_id = userId,
                msg_type = "post",
                content = new
                {
                    post = new
                    {
                        zh_cn = new
                        {
                            title = title,
                            content = new[]
                            {
                                new[]
                                {
                                    new
                                    {
                                        tag = "text",
                                        text = wrappedContent
                                    }
                                }
                            }
                        }
                    }
                }
            };

            StringContent contentJson = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://open.feishu.cn/open-apis/im/v1/messages")
            {
                Content = contentJson,
                Headers = { { "Authorization", $"Bearer {accessToken}" } }
            };

            HttpResponseMessage response = await m_HttpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            return root.GetProperty("code").GetInt32() == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"包装富文本消息失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 发送转发消息（类似PostForward功能）
    /// </summary>
    public async Task<bool> PostForward(string title, string text, string emailAddress)
    {
        try
        {
            string userId = await GetUserIdByEmailAsync(emailAddress);
            string accessToken = await GetAccessTokenAsync();

            string forwardContent = $@"【转发消息】
标题：{title}
内容：{text}
转发时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            var requestBody = new
            {
                receive_id = userId,
                msg_type = "text",
                content = new
                {
                    text = forwardContent
                }
            };

            StringContent contentJson = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://open.feishu.cn/open-apis/im/v1/messages")
            {
                Content = contentJson,
                Headers = { { "Authorization", $"Bearer {accessToken}" } }
            };

            HttpResponseMessage response = await m_HttpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            return root.GetProperty("code").GetInt32() == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发送转发消息失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    public async Task<string> UploadImage(string imagePath)
    {
        try
        {
            string accessToken = await GetAccessTokenAsync();

            using MultipartFormDataContent form = new MultipartFormDataContent();
            using FileStream fileStream = File.OpenRead(imagePath);
            using StreamContent fileContent = new StreamContent(fileStream);
                
            form.Add(fileContent, "image", Path.GetFileName(imagePath));
            form.Add(new StringContent("message"), "image_type");

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://open.feishu.cn/open-apis/im/v1/images")
            {
                Content = form,
                Headers = { { "Authorization", $"Bearer {accessToken}" } }
            };

            HttpResponseMessage response = await m_HttpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            if (root.GetProperty("code").GetInt32() != 0)
            {
                throw new Exception($"上传图片失败: {result}");
            }

            return root.GetProperty("data").GetProperty("image_key").GetString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"上传图片失败: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    public async Task<string> UploadFile(string filePath, string fileName = null)
    {
        try
        {
            string accessToken = await GetAccessTokenAsync();

            using MultipartFormDataContent form = new MultipartFormDataContent();
            await using FileStream fileStream = File.OpenRead(filePath);
            using StreamContent fileContent = new StreamContent(fileStream);
                
            form.Add(fileContent, "file", fileName ?? Path.GetFileName(filePath));
            form.Add(new StringContent("doc"), "file_type");
            form.Add(new StringContent(fileName ?? Path.GetFileName(filePath)), "file_name");

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://open.feishu.cn/open-apis/im/v1/files")
            {
                Content = form,
                Headers = { { "Authorization", $"Bearer {accessToken}" } }
            };

            HttpResponseMessage response = await m_HttpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            if (root.GetProperty("code").GetInt32() != 0)
            {
                throw new Exception($"上传文件失败: {result}");
            }

            return root.GetProperty("data").GetProperty("file_key").GetString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"上传文件失败: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 发送文件
    /// </summary>
    public async Task<bool> SendFile(string filePath, string emailAddress, string fileName = null)
    {
        try
        {
            string fileKey = await UploadFile(filePath, fileName);
            if (string.IsNullOrEmpty(fileKey))
            {
                return false;
            }

            string userId = await GetUserIdByEmailAsync(emailAddress);
            string accessToken = await GetAccessTokenAsync();

            var requestBody = new
            {
                receive_id = userId,
                msg_type = "file",
                content = new
                {
                    file_key = fileKey
                }
            };

            StringContent contentJson = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://open.feishu.cn/open-apis/im/v1/messages")
            {
                Content = contentJson,
                Headers = { { "Authorization", $"Bearer {accessToken}" } }
            };

            HttpResponseMessage response = await m_HttpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();

            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            return root.GetProperty("code").GetInt32() == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发送文件失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 发送字符串作为文件
    /// </summary>
    public async Task<bool> SendStringAsFile(string content, string fileName, string emailAddress)
    {
        try
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            await File.WriteAllTextAsync(tempFilePath, content, Encoding.UTF8);
                
            bool result = await SendFile(tempFilePath, emailAddress, fileName);
                
            // 清理临时文件
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
                
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发送字符串文件失败: {ex.Message}");
            return false;
        }
    }
}
