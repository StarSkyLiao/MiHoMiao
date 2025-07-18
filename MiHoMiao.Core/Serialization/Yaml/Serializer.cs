using YamlDotNet.Serialization;

namespace MiHoMiao.Core.Serialization.Yaml;

public static class Serializer
{
    /// <summary>
    /// 将一个 DataObject 对象转化为 YAML 字符串
    /// </summary>
    public static string ToYaml<T>(this T dataObject)
    {
        ISerializer serializer = new SerializerBuilder().Build();
        return serializer.Serialize(dataObject);
    }
    
    /// <summary>
    /// 将一个 YAML 字符串解析为 DataObject 对象
    /// </summary>
    public static T FromYaml<T>(this string yamlString)
    {
        IDeserializer deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<T>(yamlString);
    }
}