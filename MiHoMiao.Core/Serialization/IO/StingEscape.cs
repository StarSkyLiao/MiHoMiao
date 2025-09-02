using System.Text.RegularExpressions;
using MiHoMiao.Core.Collections.Unsafe;

namespace MiHoMiao.Core.Serialization.IO;

public static class StingEscape
{
    public static string Escape(this string text) => text.AsSpan().Escape();
    
    public static string Escape(this ReadOnlySpan<char> text)
    {
        using InterpolatedString escaped = new InterpolatedString();
        foreach (char c in text)
        {
            switch (c)
            {
                case '\n': escaped.Append(@"\n"); break;
                case '\t': escaped.Append(@"\t"); break;
                case '\r': escaped.Append(@"\r"); break;
                case '\"': escaped.Append(@"\"""); break;
                case '\\': escaped.Append(@"\\"); break;
                default: escaped.Append(c); break;
            }
        }
        return escaped.ToString();
    }
    

    public static string Unescape(this ReadOnlySpan<char> text) => Unescape(text.ToString());
    public static string Unescape(this string text) => Regex.Unescape(text);
    
}