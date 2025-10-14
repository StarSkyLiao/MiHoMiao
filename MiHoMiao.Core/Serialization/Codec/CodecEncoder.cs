namespace MiHoMiao.Core.Serialization.Codec;

public static class CodecEncoder
{
    /// <summary>
    /// 将指定的对象编码并写入缓冲器.
    /// </summary>
    public static void Encode<TBuffer, TSelf>(ref TBuffer buffer, TSelf value)
        where TBuffer : allows ref struct where TSelf : IBaseCodec<TBuffer, TSelf>
        => TSelf.Encode(ref buffer, value);

    /// <summary>
    /// 解析缓冲器并得到指定的对象.
    /// </summary>
    public static TSelf Decode<TBuffer, TSelf>(ref TBuffer buffer)
        where TBuffer : allows ref struct where TSelf : IBaseCodec<TBuffer, TSelf>
        => TSelf.Decode(ref buffer);

    /// <summary>
    /// 解析缓冲器并得到指定的对象.
    /// </summary>
    public static void Decode<TBuffer, TSelf>(ref TBuffer buffer, out TSelf value)
        where TBuffer : allows ref struct where TSelf : IBaseCodec<TBuffer, TSelf>
        => value = TSelf.Decode(ref buffer);
}