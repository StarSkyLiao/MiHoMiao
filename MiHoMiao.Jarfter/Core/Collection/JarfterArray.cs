using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Core.Collection;

/// <summary>
/// 一个实现了 ISpanParsable 接口的数组.
/// 数组的格式类似于: " [ 1,2,    3 , 4]   "
/// </summary>
public class JarfterArray<T>(T[] items) : ISpanParsable<JarfterArray<T>> where T : ISpanParsable<T>
{
    /// <summary>
    /// 该 JarfterArray 的只读列表
    /// </summary>
    public ReadOnlySpan<T> Items => items;

    [Pure]
    public TOther[] Cast<TOther>(Func<T, TOther> converter)
    {
        TOther[] outputArray = new TOther[items.Length];
        for (int index = 0; index < items.Length; ++index)
            outputArray[index] = converter(items[index]);
        return outputArray;
    }

    public static JarfterArray<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        s = s.Trim();
        if (s.Length < 2 || s[0] != '[' || s[^1] != ']') 
            throw new FormatException($"'{s}' is not a valid JarfterArray<{typeof(T).Name}>.");

        ReadOnlySpan<char> inner = s[1..^1].Trim();
        if (inner.IsEmpty) return new JarfterArray<T>([]);

        List<T> parsedList = [];
        foreach (string segment in SplitElements(inner))
        {
            string itemStr = segment.Trim();
            if (string.IsNullOrEmpty(itemStr)) continue;
        
            T item = IJarfterFunc.JarfterParse<T>((provider as JarfterContext)!, itemStr);
            parsedList.Add(item);
        }

        return new JarfterArray<T>(parsedList.ToArray());
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out JarfterArray<T> result)
    {
        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch (System.Exception)
        {
            result = null;
            return false;
        }
    }

    
    private static List<string> SplitElements(ReadOnlySpan<char> span)
    {
        List<string> elements = [];
        int start = 0;
        int depth = 0;

        for (int i = 0; i < span.Length; i++)
        {
            char c = span[i];
            depth += c switch
            {
                '{' or '[' or '(' => 1,
                '}' or ']' or ')' => -1,
                _ => 0
            };

            bool isLastChar = i == span.Length - 1;
            bool isSeparator = depth == 0 && (c == ',' || isLastChar);

            if (!isSeparator) continue;
            int end = isLastChar ? span.Length : i;
            ReadOnlySpan<char> part = span[start..end].Trim(',').Trim();
            if (!part.IsEmpty) elements.Add(part.ToString());
            start = i + 1;
        }

        if (depth != 0) UnBalancedArrayException.ThrowAtEndOfLine("]");
        return elements;
    }
    
    public static JarfterArray<T> Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out JarfterArray<T> result) => TryParse(s.AsSpan(), provider, out result);
}