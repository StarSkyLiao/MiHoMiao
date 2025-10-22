using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Serialization.Codec.Internal;

public abstract class UnmanagedCodec<T> : IBaseCodec<T> where T : unmanaged
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(CodecStream buffer, T value) => buffer.Write(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Decode(CodecStream buffer) => buffer.Read<T>();
    
}