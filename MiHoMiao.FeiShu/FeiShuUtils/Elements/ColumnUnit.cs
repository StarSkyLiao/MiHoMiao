using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

namespace MiHoMiao.FeiShu.FeiShuUtils.Elements;

public class ColumnUnit(ICardElement elements, int? weighted = 1) : ICardElement
{
    public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("tag", "column");
        writer.WriteString("width", weighted is not null ? "weighted": "auto");
        if (weighted is not null) writer.WriteNumber("weight", weighted.Value);
        writer.WritePropertyName("elements");
        writer.WriteStartArray();
        elements.Write(writer, options);
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}