using System.Diagnostics;
using MiHoMiao.Migxn.Exceptions.Lexer;
using MiHoMiao.Migxn.Syntax.Tokens;
using MiHoMiao.Migxn.Syntax.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

namespace MiHoMiao.Migxn.Syntax;

/// <summary>
/// 词法分析器。
/// </summary>
public class MigxnLexer
{
    private static readonly HashSet<char> s_Punctuations = [.."~!@#$%^&*()+{}|:\"<>?-=[]\\);',./'"];

    /// <summary>
    /// 从索引开始解析，把能读到的所有 token 一口气返回。
    /// </summary>
    /// <returns>返回 token 列表；如果索引无效或到结尾则返回空列表。</returns>
    public IEnumerable<MigxnToken> Parse(string input)
    {
        var position = 0;
        while (position < input.Length)
        {
            // SkipWhitespace
            while (position < input.Length && char.IsWhiteSpace(input[position])) position++;
            if (position >= input.Length) break;

            if (TryReadNumber(input, ref position, out MigxnToken result)) yield return result;
            else if (TryReadCharLiteral(input, ref position, out result)) yield return result;
            else if (TryReadStringLiteral(input, ref position, out result)) yield return result;
            else if (TryReadPunctuation(input, ref position, out result)) yield return result;
            else if (TryReadIdentifier(input, ref position, out result)) yield return result;
            // 出现未知字符，直接跳过
            else position++;
        }
    }

    // =========== 私有解析方法 ===========

    private static bool TryReadNumber(string input, ref int position, out MigxnToken token)
    {
        token = null!;
        int start = position;
        if (!char.IsDigit(input[start])) return false;
        var hasDot = false;
        while (position < input.Length && (char.IsDigit(input[position]) || input[position] is '.'))
        {
            if (input[position] == '.')
            {
                // 第二个小数点非法
                if (hasDot) break;
                hasDot = true;
            }

            position++;
        }

        if (position == start) return false;

        ReadOnlySpan<char> slice = input.AsSpan()[start..position];

        if (hasDot)
        {
            if (!double.TryParse(slice, out double @double)) return false;
            token = new FloatToken(start, input.AsMemory()[start..position], @double);
            return true;
        }

        if (!long.TryParse(slice, out long @long)) return false;
        token = new IntToken(start, input.AsMemory()[start..position], @long);
        return true;
    }

    #region 字符字面量

    private static bool TryReadCharLiteral(string input, ref int position, out MigxnToken token)
    {
        token = null!;
        if (position >= input.Length || input[position] != '\'') return false;

        int start = position;
        position++; // 跳过起始单引号

        if (position >= input.Length) throw new UnclosedStringException(start);

        char ch;
        if (input[position] == '\\')
        {
            position++; // 跳过 '\'
            (ch, int consumed) = ReadEscapeSequence(input, position);
            position += consumed;
        }
        else
        {
            ch = input[position];
            position++;
        }

        if (position >= input.Length || input[position] != '\'')
            throw new LexerException(start, "字符字面量必须以单引号结尾");

        position++; // 跳过结束单引号
        token = new CharToken(start, input.AsMemory()[start..position], ch);
        return true;
    }

    #endregion

    #region 字符串字面量
    private static bool TryReadStringLiteral(string input, ref int position, out MigxnToken token)
    {
        token = null!;
        if (position >= input.Length || input[position] != '"') return false;

        int start = position;
        position++; // 跳过起始双引号

        var sb = new System.Text.StringBuilder();

        while (position < input.Length)
        {
            char c = input[position];
            if (c == '"')
            {
                position++;
                token = new StringToken(start, input.AsMemory()[start..position], sb.ToString());
                return true;
            }

            if (c == '\\')
            {
                position++; // 跳过 '\'
                (char ch, int consumed) = ReadEscapeSequence(input, position);
                sb.Append(ch);
                position += consumed;
            }
            else
            {
                sb.Append(c);
                position++;
            }
        }

        throw new UnclosedStringException(start);
    }
    #endregion
    
    private static bool TryReadPunctuation(string input, ref int position, out MigxnToken token)
    {
        token = null!;
        if (!s_Punctuations.Contains(input[position])) return false;
        if (position + 1 >= input.Length)
        {
            token = Punctuation.Create(position, [input.AsSpan()[position]]);
            ++position;
            return true;
        }

        ReadOnlySpan<char> two = input.AsSpan().Slice(position, 2);
        ReadOnlySpan<char> result = two switch
        {
            "!=" => "!=",
            "==" => "==",
            "<=" => "<=",
            ">=" => ">=",
            _ => [input.AsSpan()[position]]
        };
        token = Punctuation.Create(position, result);
        position += result.Length;
        return true;
    }

    private static bool TryReadIdentifier(string input, ref int position, out MigxnToken token)
    {
        token = null!;
        if (position >= input.Length) return false;
        if (!IsLetter(input[position])) return false;

        int start = position;
        position++;
        while (position < input.Length && (char.IsDigit(input[position]) || IsLetter(input[position]))) position++;

        token = Identifier.Create(start, input[start..position]);
        return true;
    }

    private static bool IsLetter(char c) => char.IsLetter(c) || c is '_' || (c >= 0x4E00 && c <= 0x9FFF);

    #region 公共转义序列读取

    private static (char value, int consumed) ReadEscapeSequence(string text, int pos)
    {
        if (pos >= text.Length) throw new LexerException(pos, "转义序列不完整");

        char esc = text[pos];
        var len = 1; // 默认消费 1 个字符

        switch (esc)
        {
            case 'n': return ('\n', len);
            case 't': return ('\t', len);
            case 'r': return ('\r', len);
            case 'b': return ('\b', len);
            case 'f': return ('\f', len);
            case 'v': return ('\v', len);
            case '\\': return ('\\', len);
            case '\'': return ('\'', len);
            case '"': return ('"', len);

            // 八进制：1-3 位
            case var _ when char.IsDigit(esc) && esc is >= '0' and <= '7':
            {
                int max = Math.Min(pos + 3, text.Length);
                int i = pos;
                var value = 0;
                while (i < max && text[i] is >= '0' and <= '7')
                {
                    value = value * 8 + (text[i] - '0');
                    i++;
                }

                len = i - pos;
                return ((char)value, len);
            }

            // 十六进制：\xHH… 任意位
            case 'x' when pos + 1 < text.Length:
            {
                int i = pos + 1;
                var value = 0;
                while (i < text.Length && IsHexDigit(text[i]))
                {
                    value = value * 16 + HexValue(text[i]);
                    i++;
                }

                if (i == pos + 1) throw new LexerException(pos, "十六进制转义序列不能为空");
                len = i - pos;
                return ((char)value, len);
            }

            default:
                throw new LexerException(pos, $"未知转义序列 \\{esc}");
        }

        static bool IsHexDigit(char c) => c is >= '0' and <= '9' or >= 'A' and <= 'F' or >= 'a' and <= 'f';

        static int HexValue(char c) => c switch
        {
            >= '0' and <= '9' => c - '0',
            >= 'A' and <= 'F' => c - 'A' + 10,
            >= 'a' and <= 'f' => c - 'a' + 10,
            _ => throw new UnreachableException()
        };
    }

    #endregion
    
}