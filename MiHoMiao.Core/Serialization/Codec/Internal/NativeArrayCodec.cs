using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Serialization.Codec.Internal;

public abstract class NativeArrayCodec<T> : IBaseCodec<T[]> where T : unmanaged
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(CodecStream buffer, T[] value) => buffer.Write(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Decode(CodecStream buffer)
    {
        Span<T> span = buffer.ReadSpan<T>();
        T[] result = new T[span.Length];
        span.CopyTo(result);
        return result;
    }
}