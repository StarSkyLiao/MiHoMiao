namespace MiHoMiao.Core.Serialization.Codec;

/// <summary>
/// 面向 ref struct 缓冲区的编解码器接口.
/// 缓冲区类型 B 必须是 ref struct (典型是 Span&lt;byte&gt;)。
/// </summary>
public interface IBaseCodec<TBuffer, TSelf> where TBuffer : allows ref struct
{
    
    /// <summary>
    /// 将指定的对象编码并写入缓冲器.
    /// </summary>
    static abstract void Encode(ref TBuffer buffer, TSelf value);
    
    /// <summary>
    /// 解析缓冲器并得到指定的对象.
    /// </summary>
    static abstract TSelf Decode(ref TBuffer buffer);

}