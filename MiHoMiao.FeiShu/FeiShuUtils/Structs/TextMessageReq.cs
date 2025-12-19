using System.Text.Json.Serialization;

namespace MiHoMiao.FeiShu.FeiShuUtils.Structs;

public record struct TextMessageReq(
    [property: JsonPropertyName("text")]string Text, 
    [property: JsonPropertyName("tag")]string Tag = "text",
    [property: JsonPropertyName("un_escape")]bool UnEscape = true
);