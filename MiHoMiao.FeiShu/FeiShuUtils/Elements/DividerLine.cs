using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

namespace MiHoMiao.FeiShu.FeiShuUtils.Elements;

public class DividerLine : ICardElement
{
    public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("tag", "hr");
        writer.WriteEndObject();
    }
}