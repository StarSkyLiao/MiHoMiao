namespace MiHoMiao.Core.Serialization.Codec;

/// <summary>
/// 面向 <see cref="T:MiHoMiao.Core.Serialization.Codec.CodecStream"/> 缓冲区的编解码器接口.
/// </summary>
public interface IBaseCodec<TSelf> where TSelf : allows ref struct
{
    
    /// <summary>
    /// 将指定的对象编码并写入缓冲器.
    /// </summary>
    static abstract void Encode(CodecStream buffer, TSelf value);
    
    /// <summary>
    /// 解析缓冲器并得到指定的对象.
    /// </summary>
    static abstract TSelf Decode(CodecStream buffer);

}