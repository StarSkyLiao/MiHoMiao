namespace MiHoMiao.FeiShu.FeiShuUtils;

/// <summary>
/// 消息内容类
/// </summary>
public class MessageContent(string text)
{
    public string tag => "text";
    public bool un_escape => true;
    public string text { get; set; } = text;
}