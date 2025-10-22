using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Serialization.Codec.Internal;

public abstract class ObjectArrayCodec<TSelf, TElement> : IBaseCodec<TElement[]> where TSelf : IBaseCodec<TElement>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(CodecStream buffer, TElement[] value)
    {
        buffer.Write(value.Length);
        foreach (TElement baseCodec in value) TSelf.Encode(buffer, baseCodec);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement[] Decode(CodecStream buffer)
    {
        int length = buffer.Read<int>();
        TElement[] result = new TElement[length];
        for (int i = 0; i < length; i++) result[i] = TSelf.Decode(buffer);
        return result;
    }
}