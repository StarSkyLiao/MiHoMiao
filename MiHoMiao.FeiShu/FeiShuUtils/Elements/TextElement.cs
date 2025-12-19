using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

namespace MiHoMiao.FeiShu.FeiShuUtils.Elements;

public class TextElement(string text, bool hasMd = false, string? color = null) : ICardElement
{
    public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("tag", "div");
        writer.WritePropertyName("text");
        CardMessageConverter.WriteText(writer, options, text, hasMd, color);
        writer.WriteEndObject();
    }
}