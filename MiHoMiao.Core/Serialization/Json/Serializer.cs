﻿using Newtonsoft.Json;

namespace MiHoMiao.Core.Serialization.Json;

public static class Serializer
{
    /// <summary>
    /// 将一个 DataObject 对象转化为 Json 字符串
    /// </summary>
    public static string ToJson<T>(this T dataObject)
        => JsonConvert.SerializeObject(dataObject);

    /// <summary>
    /// 将一个 Json 字符串解析为 DataObject 对象
    /// </summary>
    public static T? FromJson<T>(this string jsonText)
        => JsonConvert.DeserializeObject<T>(jsonText);
}