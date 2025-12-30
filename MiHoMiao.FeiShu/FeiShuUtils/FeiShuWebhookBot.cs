using System.Text;
using System.Text.Json;

namespace MiHoMiao.FeiShu.FeiShuUtils;

public static class FeiShuWebhookBot
{
    /// <summary>
    /// 通过 webhook 发送消息（适用于飞书自定义机器人）
    /// </summary>
    /// <param name="webhookUrl">自定义机器人的 webhook 地址</param>
    /// <param name="messageJson">完整的消息 JSON 字符串（必须包含 msg_type 和 content）</param>
    /// <param name="httpClient">可选的 HttpClient（建议复用以避免 Socket 耗尽），若为 null 则内部创建</param>
    /// <param name="callback">可选：成功后对返回 JSON 的处理回调</param>
    /// <returns>飞书返回的原始 JSON 字符串</returns>
    public static async Task<string> SendMessageAsync(
        string webhookUrl,
        string messageJson,
        HttpClient? httpClient = null,
        Action<JsonElement>? callback = null)
    {
        if (string.IsNullOrWhiteSpace(webhookUrl))
            throw new ArgumentException("webhookUrl 不能为空", nameof(webhookUrl));
        if (string.IsNullOrWhiteSpace(messageJson))
            throw new ArgumentException("messageJson 不能为空", nameof(messageJson));

        // 建议外部传入已复用的 HttpClient，若未传则临时创建一个（不推荐频繁创建）
        var client = httpClient ?? new HttpClient();

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 发送 webhook 消息到: {webhookUrl}");
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 请求内容: {messageJson}");

        var content = new StringContent(messageJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(webhookUrl, content);

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] webhook 请求结果: {response.StatusCode}");

        string result = await response.Content.ReadAsStringAsync();

        using JsonDocument jsonDoc = JsonDocument.Parse(result);
        JsonElement root = jsonDoc.RootElement;

        int code = root.GetProperty("code").GetInt32();
        if (code != 0)
        {
            string msg = root.TryGetProperty("msg", out var msgProp) ? msgProp.GetString() ?? "" : "";
            throw new Exception($"[{DateTime.Now:HH:mm:ss}] webhook 发送消息失败: code={code}, msg={msg}, 响应: {result}");
        }

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] webhook 消息发送成功: {result}");

        callback?.Invoke(root);

        return result;
    }
}