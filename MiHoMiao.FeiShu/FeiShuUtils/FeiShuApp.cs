using System.Text;
using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;
using MiHoMiao.FeiShu.FeiShuUtils.Structs;

namespace MiHoMiao.FeiShu.FeiShuUtils;

public sealed class FeiShuApp(string appId, string appSecret) : IDisposable
{
    public string AppId { get; } = appId;
    public string AppSecret { get; } = appSecret;
    public HttpClient HttpClient { get; } = new HttpClient();
    private string? m_AccessToken;
    private DateTime m_TokenExpiry;
    
    /// <summary>
    /// 获取访问令牌
    /// </summary>
    private async Task<string> GetAccessTokenAsync()
    {
        if (m_AccessToken is not null && m_TokenExpiry > DateTime.Now) return m_AccessToken;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 正在获取新的访问令牌...");
        AccessTokenReq requestData = new AccessTokenReq(AppId, AppSecret);
        const string Address = "https://fsopen.bytedance.net/open-apis/auth/v3/tenant_access_token/internal/";
        await this.RequestData(Address, JsonSerializer.Serialize(requestData), "访问令牌", data =>
        {
            m_AccessToken = data.GetProperty("tenant_access_token").GetString()!;
            m_TokenExpiry = DateTime.Now.AddSeconds(data.GetProperty("expire").GetInt32() - 60);
        });
        return m_AccessToken!;
    }
    
    /// <summary>
    /// 发送消息
    /// </summary>
    public void SendMessage(string title, string text, string emailAddress) => SendRichTextAsync(title, new TextMessageReq(text), emailAddress).Wait();

    /// <summary>
    /// 发送富文本消息
    /// </summary>
    public void SendRichText(string title, TextMessageReq content, string emailAddress) => SendRichTextAsync(title, content, emailAddress).Wait();
    
    public async Task<string> UploadFileAsync(string filePath, string? fileName = null)
    {
        string accessToken = await GetAccessTokenAsync();

        using MultipartFormDataContent form = new MultipartFormDataContent();
        await using FileStream fileStream = File.OpenRead(filePath);
        using StreamContent fileContent = new StreamContent(fileStream);
            
        form.Add(fileContent, "file", fileName ?? Path.GetFileName(filePath));
        form.Add(new StringContent("doc"), "file_type");
        form.Add(new StringContent(fileName ?? Path.GetFileName(filePath)), "file_name");

        const string Address = "https://fsopen.bytedance.net/open-apis/im/v1/files";
        HttpRequestMessage requestData = new HttpRequestMessage(HttpMethod.Post, Address);
        requestData.Content = form;
        requestData.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

        return await this.PostData(Address, requestData, "文件上传");
    }
    
    public async Task<string> UploadImageAsync(string filePath, string? fileName = null)
    {
        string accessToken = await GetAccessTokenAsync();

        using MultipartFormDataContent form = new MultipartFormDataContent();
        await using FileStream fileStream = File.OpenRead(filePath);
        using StreamContent fileContent = new StreamContent(fileStream);

        form.Add(fileContent, "image", fileName ?? Path.GetFileName(filePath));
        form.Add(new StringContent("message"), "image_type");

        const string Address = "https://fsopen.bytedance.net/open-apis/image/v4/put/";
        HttpRequestMessage requestData = new HttpRequestMessage(HttpMethod.Post, Address);
        requestData.Content = form;
        requestData.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
        
        string result = null!;
        await this.PostData(Address, requestData, "文件上传", root =>
            result = root.GetProperty("data").GetProperty("image_key").GetString()!
        );
        return result;
    }
    
    private async Task SendRichTextAsync(string title, TextMessageReq content, string emailAddress)
    {
        string accessToken = await GetAccessTokenAsync();

        RichTextReq richText = NetHelper.WrapRichMsg(title, [[content]], emailAddress);
        string json = JsonSerializer.Serialize(richText);
        if (emailAddress.StartsWith("https://open.larkoffice.com/open-apis/bot/v2/hook/"))
        {
            await FeiShuWebhookBot.SendMessageAsync(emailAddress, json);
            return;
        }
        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        const string Address = "https://fsopen.bytedance.net/open-apis/message/v4/send/";
        // 创建 HttpRequestMessage
        using HttpRequestMessage requestData = new HttpRequestMessage(HttpMethod.Post, Address);
        requestData.Content = stringContent;
        requestData.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
        requestData.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        
        await this.PostData(Address, requestData, "富文本信息");
    }

    /// <summary>
    /// 根据卡片构建器对象发送交互式卡片消息
    /// </summary>
    public async Task SendCardMessageAsync(CardMessage card, string receiveId)
    {
        string accessToken = await GetAccessTokenAsync();
        if (receiveId.Contains('@')) card.EmailAddress = receiveId;
        else card.OpenChatId = receiveId;
        
        string json = JsonSerializer.Serialize(card);
        if (receiveId.StartsWith("https://open.larkoffice.com/open-apis/bot/v2/hook/"))
        {
            await FeiShuWebhookBot.SendMessageAsync(receiveId, json);
            return;
        }
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        const string Address = "https://fsopen.bytedance.net/open-apis/message/v4/send/";

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Address);
        request.Content = content;
        request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

        await this.PostData(Address, request, "卡片消息");
    }
        
    public void Dispose() => HttpClient.Dispose();
    
}