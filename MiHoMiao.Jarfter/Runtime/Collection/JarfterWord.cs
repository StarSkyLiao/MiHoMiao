using System.Runtime.CompilerServices;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;

[assembly:InternalsVisibleTo("MiHoMiao.xUnit")]
[assembly: InternalsVisibleTo("MiHoMiao.Program")]

namespace MiHoMiao.Jarfter.Runtime.Collection;

public class JarfterWord(string content) : JarfterObject, IJarfterParsable<JarfterWord>
{
    public string Content => content;

    public override string ToString() => content;
    
    public void As<T>() where T : IParsable<T> => T.Parse(Content, null);

    internal static JarfterWord ParseInternal(ReadOnlySpan<char> input) => Parse(input, new JarfterContext(null!));

    public new static JarfterWord Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
        => new JarfterWord(ParseSpan(input, provider).ToString());
    
    public static ReadOnlySpan<char> ParseSpan(ReadOnlySpan<char> input, IFormatProvider? provider)
    {
        if (provider is not JarfterContext context) return DefaultParseSpan(input);
        ref int index = ref context.ParsingIndex;
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        int start = index;
        while (index < input.Length && !char.IsWhiteSpace(input[index]) && !IsPunctuation(input[index])) ++index;
        context.ParsingIndex = index;
        return input[start..index].ToString();
    }
    
    private static ReadOnlySpan<char> DefaultParseSpan(ReadOnlySpan<char> input)
    {
        int index = 0;
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        int start = index;
        while (index < input.Length && !char.IsWhiteSpace(input[index]) && !IsPunctuation(input[index])) ++index;
        if (start == index) throw new InvalidCallingTreeException();
        return input[start..index].ToString();
    }
    
    internal static bool IsPunctuation(char input) => char.IsPunctuation(input) && (input is not '.' and not '_' and not '%');
    
}