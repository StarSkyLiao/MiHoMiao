using System.Text.Json;

namespace MiHoMiao.FeiShu.FeiShuUtils.InteractiveCards;

public interface ICardElement
{
    public void Write(Utf8JsonWriter writer, JsonSerializerOptions options);
}