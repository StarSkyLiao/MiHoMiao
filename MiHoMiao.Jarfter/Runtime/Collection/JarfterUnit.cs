using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Runtime.Collection;

public class JarfterUnit(object content) : JarfterObject, IJarfterParsable<JarfterUnit>
{
    public object Content => content;
    
    public new static JarfterUnit Parse(ReadOnlySpan<char> input, IFormatProvider? provider = null)
    {
        object? result = null;
        if (TryParseJarfterParsable(input, out bool? @bool)) result = @bool;
        else if (TryParseJarfterParsable(input, out decimal? @decimal)) result = @decimal;
        else if (TryParseJarfterParsable(input, out long? @long)) result = @long;
        
        if (result == null) throw new InvalidTypeException<JarfterUnit>(input.ToString());
        return new JarfterUnit(result);
    }

    private static bool TryParseJarfterParsable<T>(ReadOnlySpan<char> input, out T? result)
        where T : unmanaged, ISpanParsable<T>
    {
        if (T.TryParse(input, null, out T item))
        {
            result = item;
            return true;
        }
        result = null;
        return false;
    }

    public override string ToString() => content.ToString()!;
    
}