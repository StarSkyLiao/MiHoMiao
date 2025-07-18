using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Jarfter.Core.Collection;

/// <summary>
/// 一个实现了 ISpanParsable 接口的字典.
/// 字典的格式类似于: "{ a = 1, b = true, c = {1, 2, 3}, d = { e = 1, f = 2}}"
/// </summary>
public class JarfterDictionary(string input) : ISpanParsable<JarfterDictionary>
{
    private readonly Dictionary<string, ValueSlice> m_Dict = new Dictionary<string, ValueSlice>();
    
    /// <summary>
    /// 该 JarfterDictionary 的只读字典
    /// </summary>
    public IReadOnlyDictionary<string, ValueSlice> Items => m_Dict;
    
    /// <summary>
    /// 该 JarfterDictionary 的只读键集合
    /// </summary>
    public Dictionary<string, ValueSlice>.KeyCollection Keys => m_Dict.Keys;

    /// <summary>
    /// 读取 key 对应的值, 并且解析为 T 类型
    /// </summary>
    public T? ReadAs<T>(string key, IFormatProvider? provider = null) where T : ISpanParsable<T> 
        => m_Dict.TryGetValue(key, out var value) ? T.Parse(value.AsSpan(input), provider) : default;

    #region ISpanParsable

    public static JarfterDictionary Parse(string s, IFormatProvider? _) => Parse(s.AsSpan(), _);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? _, [MaybeNullWhen(false)] out JarfterDictionary result)
    {
        if (s is not null) return TryParse(s.AsSpan(), _, out result);
        result = null;
        return false;
    }

    public static JarfterDictionary Parse(ReadOnlySpan<char> s, IFormatProvider? _)
    {
        if (!TryParse(s, _, out var d)) throw new FormatException("Invalid dictionary.");
        return d;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? _, [MaybeNullWhen(false)] out JarfterDictionary result)
    {
        result = null;
        s = s.Trim();
        if (s.IsEmpty || s[0] != '{' || s[^1] != '}') return false;

        var jarfterDictionary = new JarfterDictionary(s.ToString());
        ReadOnlySpan<char> inner = s[1..^1].Trim();
        if (inner.IsEmpty)
        {
            result = jarfterDictionary;
            return true;
        }

        int pos = 0;
        while (true)
        {
            // key
            SkipWhite(inner, ref pos);
            if (pos >= inner.Length) break;
            if (!TryReadKey(inner, ref pos, out ReadOnlySpan<char> keySpan)) return false;
            var key = keySpan.ToString();

            // '='
            SkipWhite(inner, ref pos);
            if (pos >= inner.Length || inner[pos] != '=') return false;
            pos++; // skip '='

            // value
            SkipWhite(inner, ref pos);
            if (pos >= inner.Length) return false;

            var valueStart = pos + s.Length - inner.Length; // 计算原始字符串中的绝对起始
            if (!TrySkipValue(inner, ref pos)) return false;
            var valueLen = pos + s.Length - inner.Length - valueStart;

            jarfterDictionary.m_Dict[key] = new ValueSlice(valueStart - 1, valueLen);

            SkipWhite(inner, ref pos);
            if (pos >= inner.Length) break;
            if (inner[pos] != ',') return false; // 只能是逗号或结尾
            pos++;
        }

        result = jarfterDictionary;
        return true;
    }

    #endregion

    #region PrivateHelper

    private static void SkipWhite(ReadOnlySpan<char> span, ref int p)
    {
        while (p < span.Length && char.IsWhiteSpace(span[p])) p++;
    }

    private static bool TryReadKey(ReadOnlySpan<char> span, ref int p, out ReadOnlySpan<char> key)
    {
        int start = p;
        if (span[p] == '\"')
        {
            p++; // skip "
            while (p < span.Length && span[p] != '"') p++;
            if (p >= span.Length) { key = default; return false; }
            key = span.Slice(start + 1, p - start - 1);
            p++; // skip closing "
            return true;
        }

        while (p < span.Length && !char.IsWhiteSpace(span[p]) && span[p] != '=') p++;
        key = span.Slice(start, p - start);
        return !key.IsEmpty;
    }

    // 跳过完整值：基本值 / 对象 / 数组
    private static bool TrySkipValue(ReadOnlySpan<char> span, ref int p)
    {
        SkipWhite(span, ref p);
        if (p >= span.Length) return false;

        char c = span[p];
        return c switch
        {
            '"' => SkipString(span, ref p),
            '{' => SkipBraced(span, ref p, '{', '}'),
            '[' => SkipBraced(span, ref p, '[', ']'),
            _ => SkipLiteral(span, ref p)
        };
    }

    private static bool SkipString(ReadOnlySpan<char> span, ref int p)
    {
        p++; // skip "
        while (p < span.Length)
        {
            if (span[p] == '\\') { p++; if (p >= span.Length) return false; }
            else if (span[p] == '"') { p++; return true; }
            p++;
        }
        return false;
    }

    private static bool SkipBraced(ReadOnlySpan<char> span, ref int p, char open, char close)
    {
        int depth = 1;
        p++; // skip open
        while (p < span.Length)
        {
            if (span[p] == open) depth++;
            else if (span[p] == close)
            {
                depth--;
                if (depth == 0) { p++; return true; }
            }
            else if (span[p] == '"') { if (!SkipString(span, ref p)) return false; continue; }
            p++;
        }
        return false;
    }

    private static bool SkipLiteral(ReadOnlySpan<char> span, ref int p)
    {
        while (p < span.Length && span[p] != ',' && span[p] != '}' && span[p] != ']' && !char.IsWhiteSpace(span[p]))
            p++;
        return true;
    }

    #endregion
    
    public readonly record struct ValueSlice(int Start, int Length)
    {
        public ReadOnlySpan<char> AsSpan(ReadOnlySpan<char> source) => source.Slice(Start, Length);
    }
    
}