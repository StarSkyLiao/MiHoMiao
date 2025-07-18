using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Specialized;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Core;

public class JarfterInterpreter()
{
    [field: AllowNull, MaybeNull]
    internal JarfterContext JarfterContext => field ??= new JarfterContext(this);

    public Stack<object?> CalculationStack => JarfterContext.CalculationStack;
    
    public JarfterSymbolTable<object> SymbolTable => JarfterContext.JarfterSymbolTable;
    
    public void Run(string[] input)
    {
        int length = input.Sum(str => str.Length);
        const string Leader = "block [";
        const string Tail = "]";
        length += Leader.Length + Tail.Length + input.Length * 3;

        using DynamicString dynamicString = new DynamicString(length);
        dynamicString.Append(Leader);
        foreach (string item in input)
        {
            dynamicString.AppendFormattable('{');
            dynamicString.Append(item);
            dynamicString.AppendFormattable('}');
            dynamicString.AppendFormattable(',');
        }
        dynamicString.Append(Tail);
        Run(dynamicString);
    }
    
    public void Run(ReadOnlySpan<char> input)
    {
        string[] token = SplitStringRespectingBraces(input);
        IJarfterFunc? jarfterFunc = JarfterCollector.LoadFunc(token[0]);
        if (jarfterFunc == null) throw new FunctionNotFoundException(token[0]);
        jarfterFunc.RunJarfterFunc(JarfterContext, token.AsSpan()[1..]);
    }
        
    public static string[] SplitStringRespectingBraces(ReadOnlySpan<char> input)
    {
        List<string> result = [];
        int i = 0;
        int length = input.Length;

        while (i < length)
        {
            // 跳过前导空格
            while (i < length && char.IsWhiteSpace(input[i])) i++;

            if (i >= length) break;

            int start = i;

            // 检测是否以 @{ 或 @[ 开头
            bool isAtBrace = false;
            if (input[i] == '@' && i + 1 < length &&
                (input[i + 1] == '{' || input[i + 1] == '['))
            {
                isAtBrace = true;
                i += 2;   // 跳过 '@' 和 '{'/'['
            }
            else if (input[i] is '{' or '[')
            {
                i++;
            }

            // 进入括号配对逻辑
            if (isAtBrace || input[start] is '{' or '[')
            {
                int braceCount = 1;
                while (i < length && braceCount > 0)
                {
                    switch (input[i])
                    {
                        case '{' or '[':
                            braceCount++;
                            break;
                        case '}' or ']':
                            braceCount--;
                            break;
                    }
                    i++;
                }

                if (braceCount != 0)
                    throw new ArgumentException("未闭合的大括号或方括号");
            }
            else
            {
                // 普通 token：遇到空白即结束
                while (i < length && !char.IsWhiteSpace(input[i])) i++;
            }

            ReadOnlySpan<char> token = input.Slice(start, i - start).Trim();
            result.Add(token.ToString());
        }

        return result.ToArray();
    }
}