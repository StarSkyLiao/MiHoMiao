using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Jarfter.Core.Collection;

/// <summary>
/// 一个实现了 ISpanParsable 接口的对象.
/// 调用<see cref="JarfterObject.As"/>来按照其他的格式进行转化.
/// </summary>
public class JarfterObject(string rawString, IFormatProvider? provider = null) : ISpanParsable<JarfterObject>
{
    /// <summary>
    /// 该 JarfterObject 对应的原始字面字符串
    /// </summary>
    public string RawString => rawString;

    /// <summary>
    /// 转为为指定的实现了 ISpanParsable 接口的对象
    /// </summary>
    public T As<T>() where T : ISpanParsable<T> => T.Parse(RawString, provider);

    public override string ToString() => rawString;

    public static JarfterObject Parse(string s, IFormatProvider? provider) 
        => new JarfterObject(s, provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out JarfterObject result)
    {
        result = new JarfterObject(s, provider);
        return true;
    }

    public static JarfterObject Parse(ReadOnlySpan<char> s, IFormatProvider? provider) 
        => new JarfterObject(s.ToString(), provider);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out JarfterObject result)
    {
        result = new JarfterObject(s.ToString(), provider);
        return true;
    }
    
}