using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Jarfter.Runtime.Collection;

public interface IJarfterParsable<T> : ISpanParsable<T> where T : ISpanParsable<T>?
{
    
    static T IParsable<T>.Parse(string s, IFormatProvider? provider) => T.Parse(s.AsSpan(), provider);
    
    static bool ISpanParsable<T>.TryParse(ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T result)
    {
        try
        {
            result = T.Parse(s, provider);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
    
    static bool IParsable<T>.TryParse(
        [NotNullWhen(true)] string? s, 
        IFormatProvider? provider, 
        [MaybeNullWhen(false)] out T result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }
        try
        {
            result = T.Parse(s, provider);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}