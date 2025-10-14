namespace MiHoMiao.Core.Serialization.Codec.Internal;

public readonly struct Int32Codec : IBaseCodec<Span<byte>, int>
{
    public static void Encode(ref Span<byte> buffer, int value)
    {
        BitConverter.TryWriteBytes(buffer, value);
        buffer = buffer[sizeof(int)..];
    }
    public static int Decode(ref Span<byte> buffer)
    {
        int v = BitConverter.ToInt32(buffer);
        buffer = buffer[sizeof(int)..];
        return v;
    }

}