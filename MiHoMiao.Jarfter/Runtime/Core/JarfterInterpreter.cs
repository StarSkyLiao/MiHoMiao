using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Specialized;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Function.Main;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Core;

public class JarfterInterpreter()
{
    [field: AllowNull, MaybeNull] internal JarfterContext JarfterContext => field ??= new JarfterContext(this);
    
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
        JarfterContext.ParsingIndex = 0;
        ref int index = ref JarfterContext.ParsingIndex;
        while (char.IsWhiteSpace(input[index])) ++index;
        // $[] 表示简写的 block 语句
        if (input[index] is '$')
        {
            index += 2;
            string[] commands = JarfterFunc.SplitByCommaWithBrackets(input, index, out int tail);
            index = tail + 1;
            BlockStmt.JarfterFuncImpl(JarfterContext, new JarfterArray<string>(commands));
        }
        else
        {
            JarfterWord funcName = JarfterWord.Parse(input, JarfterContext);
            IJarfterFunc? jarfterFunc = JarfterCollector.LoadFunc(funcName.Content);
            if (jarfterFunc == null) throw new FunctionNotFoundException(funcName.Content);
            jarfterFunc.RunJarfterFunc(JarfterContext, input);
        }

    }
    
}