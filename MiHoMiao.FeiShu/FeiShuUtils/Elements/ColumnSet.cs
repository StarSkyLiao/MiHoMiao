using System.Text.Json;
using MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

namespace MiHoMiao.FeiShu.FeiShuUtils.Elements;

public class ColumnSet(ColumnUnit[] units) : ICardElement
{
    public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("tag", "column_set");
        writer.WriteString("flex_mode", "none");
        writer.WriteString("background_style", "default");
        writer.WriteString("horizontal_spacing", "default");
        writer.WritePropertyName("columns");
        writer.WriteStartArray();
        foreach (ColumnUnit item in units) item.Write(writer, options);
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}