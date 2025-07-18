using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Collection;

public class JarfterObject : IJarfterParsable<JarfterObject>
{
    
    public static JarfterObject Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
    {
        JarfterObject? result =
            (JarfterObject?)ParseJarfterParsable<JarfterUnit>(input, provider) ??
            (JarfterObject?)ParseJarfterParsable<JarfterWord>(input, provider) ??
            (JarfterObject?)ParseJarfterParsable<JarfterFunc>(input, provider) ??
            ParseJarfterParsable<JarfterArray<JarfterObject>>(input, provider);
        if (result == null) throw new InvalidTypeException<JarfterObject>(input.ToString());
        return result;
    }

    private static T? ParseJarfterParsable<T>(ReadOnlySpan<char> input, IFormatProvider? provider)
        where T : ISpanParsable<T> 
        => T.TryParse(input, provider, out T? result) ? result : default;

    public static T Convert<T>(object? variable, string name, JarfterContext jarfterContext) where T : ISpanParsable<T>
    {
        if (variable is null) throw new InvalidTypeException<T>(name);
        if (variable is T item) return item;
        return T.Parse(variable.ToString(), jarfterContext);
    }     
    
    public static T ElementParse<T>(ReadOnlySpan<char> input, JarfterContext context) where T : ISpanParsable<T>
    {
        T result;
        if (typeof(T).IsAssignableTo(typeof(IJarfterParsable<T>)))
        {
            result = T.Parse(input, context);
        }
        else
        {
            int start = context.ParsingIndex;
            while (start < input.Length && char.IsWhiteSpace(input[start])) ++start;
            int end = start;
            while (end < input.Length && !char.IsWhiteSpace(input[end]) && input[end] is not ',' and not ']') ++end;
            result = T.Parse(input[start..end], context);
            context.ParsingIndex = end;
        }

        return result;
    }
    
}