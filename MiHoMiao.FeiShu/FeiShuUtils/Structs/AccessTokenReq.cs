using System.Text.Json.Serialization;

namespace MiHoMiao.FeiShu.FeiShuUtils.Structs;

public record struct AccessTokenReq(
    [property: JsonPropertyName("app_id")]string AppId, 
    [property: JsonPropertyName("app_secret")]string AppSecret
);