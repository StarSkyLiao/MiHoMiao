using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Serialization.Codec.Internal;

public abstract class StringCodec : IBaseCodec<string>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(CodecStream buffer, string value) => buffer.Write(value.AsSpan());
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Decode(CodecStream buffer) => new string(buffer.ReadSpan<char>());

}