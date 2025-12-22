using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

[JsonConverter(typeof(CardMessageConverter))]
public class CardMessage(string title, string? subTitle = null, string? template = null)
{
    public string? EmailAddress { get; set; }
    public string? OpenChatId { get; set; }
    public static string MsgType => "interactive";
    public string Title { get; set; } = title;
    public string? SubTitle { get; set; } = subTitle;
    public string? TemplateColor { get; set; } = template;

    public List<ICardElement> Elements { get; } = [];
}


public sealed class CardMessageConverter : JsonConverter<CardMessage>
{
    public override void Write(Utf8JsonWriter writer, CardMessage value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.EmailAddress != null) writer.WriteString("email", value.EmailAddress);
        if (value.OpenChatId != null) writer.WriteString("open_chat_id", value.OpenChatId);
        writer.WriteString("msg_type", CardMessage.MsgType);
        writer.WritePropertyName("card");
        WriteCard(writer, value, options);
        writer.WriteEndObject();
    }
    
    public static void WriteCard(Utf8JsonWriter writer, CardMessage value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("config");
        {
            writer.WriteStartObject();
            writer.WriteBoolean("enable_forward", true);
            writer.WriteEndObject();
        }
        writer.WritePropertyName("header");
        WriteHeader(writer, value, options);
        writer.WritePropertyName("elements");
        WriteElems(writer, value, options);
        writer.WriteEndObject();
    }
    
    public static void WriteHeader(Utf8JsonWriter writer, CardMessage value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("title");
        WriteText(writer, options, value.Title);
        if (value.SubTitle != null)
        {
            writer.WritePropertyName("subtitle");
            WriteText(writer, options, value.SubTitle);
        }
        if (value.TemplateColor is not null) writer.WriteString("template", value.TemplateColor);
        writer.WriteEndObject();
    }
    
    public static void WriteText(Utf8JsonWriter writer, JsonSerializerOptions options, string text, bool hasMd = false, string? color = null)
    {
        writer.WriteStartObject();
        writer.WriteString("tag", hasMd ? "lark_md" : "plain_text");
        if (color != null) writer.WriteString("text_color", color);
        writer.WriteString("content", text);
        writer.WriteEndObject();
    }
    
    public static void WriteElems(Utf8JsonWriter writer, CardMessage value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (ICardElement item in value.Elements) item.Write(writer, options);
        writer.WriteEndArray();
    }

    public override CardMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("仅支持序列化");
}