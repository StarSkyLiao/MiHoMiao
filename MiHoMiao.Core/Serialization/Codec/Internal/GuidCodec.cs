namespace MiHoMiao.Core.Serialization.Codec.Internal;

public readonly struct GuidCodec : IBaseCodec<Span<byte>, Guid>
{
    public static void Encode(ref Span<byte> buffer, Guid value)
    {
        value.TryWriteBytes(buffer);
        buffer = buffer[16..];
    }
    
    public static Guid Decode(ref Span<byte> buffer)
    {
        var g = new Guid(buffer[..16]);
        buffer = buffer[16..];
        return g;
    }

}