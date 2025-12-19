using System.Net;
using System.Text;
using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.Structs;

namespace MiHoMiao.FeiShu.FeiShuUtils;

internal static class NetHelper
{
    extension(FeiShuApp feiShuApp)
    {
        public async Task<string> RequestData(string requestUri, string stringContent, string target, Action<JsonElement>? callback)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 发送{target}请求到: {requestUri}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 请求内容: {stringContent}))");
        
            StringContent content = new StringContent(stringContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await feiShuApp.HttpClient.PostAsync(requestUri, content);
        
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 请求{target}数据的结果: {response.StatusCode}");
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception(response.ReasonPhrase);
        
            string result = await response.Content.ReadAsStringAsync();
            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            if (root.GetProperty("code").GetInt32() != 0) throw new Exception($"[{DateTime.Now:HH:mm:ss}] 获取{target}失败: {result}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {target}数据发送的回复: {result}");
            callback?.Invoke(root);
            return result;
        }
        
        public async Task<string> PostData(string requestUri, HttpRequestMessage data, string target, Action<JsonElement>? callback = null)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 发送{target}数据到: {requestUri}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 数据内容: {data}");
            
            HttpResponseMessage response = await feiShuApp.HttpClient.SendAsync(data);
        
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {target}数据发送的结果: {response.StatusCode}");
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception(response.ReasonPhrase);
        
            string result = await response.Content.ReadAsStringAsync();
            using JsonDocument jsonDoc = JsonDocument.Parse(result);
            JsonElement root = jsonDoc.RootElement;

            if (root.GetProperty("code").GetInt32() != 0) throw new Exception($"[{DateTime.Now:HH:mm:ss}] {target}数据发送失败: {result}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {target}数据发送的回复: {result}");
            callback?.Invoke(root);
            return result;
        }
        
    }
    
    public static RichTextReq WrapRichMsg(string title, TextMessageReq[][] content, string receiveId) 
        => receiveId.Contains('@') ? new RichTextReq(title, content, Email: receiveId): new RichTextReq(title, content, ChatId: receiveId);
}