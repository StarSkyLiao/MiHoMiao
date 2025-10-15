namespace MiHoMiao.Core.Serialization.Codec;

/// <summary>
/// 基于 <see cref="T:MiHoMiao.Core.Serialization.Codec.CodecStream"/> 的编码.
/// 对于实现了 <see cref="T:MiHoMiao.Core.Serialization.Codec.IBaseCodec"/>接口的对象,
/// 使用 Codec 可以将其二进制序列化到字节流中, 并从二进制字节流中读取并解析出完整对象.
/// 这种方式提供了比基于 string 的编码方式(例如:Json, Yaml等)更高效的速度与更优化的存储空间,
/// 但是相应地, 具有较低的灵活性与几乎人类不可读的序列化结果.
/// </summary>
public static class CodecEncoder
{
    /// <summary>
    /// 将指定的对象编码并写入缓冲器.
    /// </summary>
    public static CodecStream Encode<TSelf>(TSelf value) where TSelf : IBaseCodec<TSelf>
    {
        CodecStream buffer = new CodecStream();
        TSelf.Encode(buffer, value);
        return buffer;
    }

    /// <summary>
    /// 将指定的对象编码并写入缓冲器.
    /// </summary>
    public static void Encode<TSelf>(this CodecStream buffer, TSelf value) where TSelf : IBaseCodec<TSelf>
        => TSelf.Encode(buffer, value);

    /// <summary>
    /// 解析缓冲器并得到指定的对象.
    /// </summary>
    public static TSelf Decode<TSelf>(this CodecStream buffer) where TSelf : IBaseCodec<TSelf> => TSelf.Decode(buffer);

    /// <summary>
    /// 解析缓冲器并得到指定的对象.
    /// </summary>
    public static void Decode<TSelf>(this CodecStream buffer, out TSelf value) where TSelf : IBaseCodec<TSelf> => value = TSelf.Decode(buffer);
}