using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Collection;

/// <summary>
/// 参数 funcCode 为没有被 {} 包围的原始代码
/// </summary>
public class JarfterFunc(string funcCode) : JarfterObject, IJarfterParsable<JarfterFunc>
{
    public string FuncCode => funcCode;

    public new static JarfterFunc Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
    {
        if (provider is not JarfterContext context) throw new InvalidCallingTreeException();
        if (input[context.ParsingIndex] is '$')
        {
            int start = context.ParsingIndex;
            ++context.ParsingIndex;
            if (input[context.ParsingIndex] is not '{')
                throw new InvalidTypeException<JarfterFunc>(input[context.ParsingIndex..].ToString());
            context.ParsingIndex = FunctionTail(input, context.ParsingIndex);
            return new JarfterFunc($"$[{input[(start + 2)..(context.ParsingIndex - 1)]}]");
        }
        if (input[context.ParsingIndex] is '-')
        {
            int start = context.ParsingIndex;
            ++context.ParsingIndex;
            if (input[context.ParsingIndex] is not '>')
                throw new InvalidTypeException<JarfterFunc>(input[context.ParsingIndex..].ToString());
            context.ParsingIndex = FunctionTail(input, context.ParsingIndex);
            return new JarfterFunc(input[(start + 2)..(context.ParsingIndex - 1)].ToString());
        }
        return BracketParse(input, provider, '{', '}');
    }

    public override string ToString()
    {
        return $"@{{{funcCode}}}";
    }

    internal static JarfterFunc ParseInternal(ReadOnlySpan<char> input)
    {
        return Parse(input, new JarfterContext(null!));
    }

    public static JarfterFunc BracketParse(ReadOnlySpan<char> input, IFormatProvider? provider, char open, char close)
    {
        if (provider is not JarfterContext context) throw new InvalidCallingTreeException();
        int index = context.ParsingIndex;
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        int start = index;
        if (input[index++] != open) throw new UnBalancedArrayException(open.ToString(), input[index..].ToString());

        int depth = 1;
        while (index < input.Length)
        {
            char letter = input[index++];
            if (letter == open) 
                ++depth;
            else if (letter == close) 
                --depth;

            if (depth == 0) break;
            if (depth < 0) throw new UnBalancedArrayException("???", close.ToString());
        }

        if (depth > 0) UnBalancedArrayException.ThrowAtEndOfLine(close.ToString());
        int end = index;
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        context.ParsingIndex = index;
        return new JarfterFunc(input[(start + 1)..(end - 1)].ToString());
    }

    internal static int FunctionTail(ReadOnlySpan<char> input, int head)
    {
        int depth = 0;
        int tail = 0;
        for (tail = head; tail < input.Length; tail++)
        {
            char c = input[tail];

            switch (c)
            {
                case '(' or '{' or '[':
                {
                    depth++;
                    break;
                }
                case ')' or '}' or ']':
                {
                    depth--;
                    break;
                }
            }
            if (depth == 0)
            {
                if (c is ',' or ';') return tail + 1;
            }

            if (depth == -1) break;
        }
        if (depth == 0) return tail + 1;
        return -1;
    }

    internal static string[] SplitByCommaWithBrackets(ReadOnlySpan<char> input, int head, out int tail)
    {
        List<string> result = [];
        int depth = 0;
        ReadOnlySpan<char> segment;
        
        for (tail = head; tail < input.Length; tail++)
        {
            char c = input[tail];

            switch (c)
            {
                case '(' or '{' or '[':
                {
                    depth++;
                    break;
                }
                case ')' or '}' or ']':
                {
                    depth--;
                    if (depth != -1) break;
                    segment = input[head..tail].Trim();
                    if (segment.Length > 0) result.Add(segment.ToString());
                    head = tail + 1;
                    break;
                }
                case ',' when depth == 0:
                {
                    segment = input[head..tail].Trim();
                    if (segment.Length > 0) result.Add(segment.ToString());
                    head = tail + 1;
                    break;
                }
            }
            
            if (depth == -1) break;
        }

        if (depth > 0) UnBalancedArrayException.ThrowAtEndOfLine("} or ] or )");
        if (head >= tail) return result.ToArray();
        segment = input[head..tail].Trim();
        if (segment.Length > 0) result.Add(segment.ToString());
        return result.ToArray();
    }
}