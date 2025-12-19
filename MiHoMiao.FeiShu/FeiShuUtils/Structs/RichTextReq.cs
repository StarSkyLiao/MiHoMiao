using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiHoMiao.FeiShu.FeiShuUtils.Structs;

[JsonConverter(typeof(RichTextReqConverter))]
public record struct RichTextReq(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("content")] TextMessageReq[][] Content,
    [property: JsonPropertyName("msg_type")] string MsgType = "post",
    [property: JsonPropertyName("chat_id")] string? ChatId = null,
    [property: JsonPropertyName("email")] string? Email = null
);

public sealed class RichTextReqConverter : JsonConverter<RichTextReq>
{
    public override void Write(Utf8JsonWriter writer, RichTextReq value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("msg_type", value.MsgType); // "msg_type":"post"
        writer.WritePropertyName("content");       // "content":
        writer.WriteStartObject();                 //   {
        writer.WritePropertyName("post");          //     "post":
        writer.WriteStartObject();                 //       {
        writer.WritePropertyName("zh_cn");         //         "zh_cn":
        writer.WriteStartObject();                 //           {
        writer.WriteString("title", value.Title);           //             "title":""
        writer.WritePropertyName("content");       //             "content":
        JsonSerializer.Serialize(writer, value.Content, options); // 复用系统序列化二维数组
        writer.WriteEndObject();                   //           }
        writer.WriteEndObject();                   //       }
        writer.WriteEndObject();                   //   }

        if (value.ChatId is not null) writer.WriteString("chat_id", value.ChatId);
        if (value.Email is not null) writer.WriteString("email", value.Email);

        writer.WriteEndObject();                   // }
    }

    public override RichTextReq Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("仅支持序列化");
}