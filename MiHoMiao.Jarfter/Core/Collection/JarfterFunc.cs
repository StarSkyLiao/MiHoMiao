using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Jarfter.Core.Collection;

public class JarfterFunc(string funcCode) : ISpanParsable<JarfterFunc>
{
    public string FuncCode => funcCode;
    
    internal static JarfterFunc ParseInternal(ReadOnlySpan<char> input) => Parse(input, new JarfterContext(null!));

    public static JarfterFunc Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out JarfterFunc result) => TryParse(s.AsSpan(), provider, out result);

    public static JarfterFunc Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out JarfterFunc? result)
            ? result
            : throw new FormatException($"{{>>{s}<<}}' is not a valid JarfterFunc.");

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out JarfterFunc result)
    {
        bool parseResult = s[0] is '{' && s[^1] is '}';
        result = parseResult ? new JarfterFunc(s[1..^1].ToString()) : null;
        return parseResult;
    }
}