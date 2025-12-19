using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

namespace MiHoMiao.FeiShu.FeiShuUtils.Elements;

public class ImageElement(string imgKey, string? title = null) : ICardElement
{
    public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("tag", "img");
        writer.WriteString("img_key", imgKey);
        writer.WriteString("mode", "crop_center");
        writer.WriteBoolean("preview", true);
        writer.WriteBoolean("compact_width", true);
        if (title != null) CardMessageConverter.WriteText(writer, options, title);
        writer.WriteEndObject();
    }
}