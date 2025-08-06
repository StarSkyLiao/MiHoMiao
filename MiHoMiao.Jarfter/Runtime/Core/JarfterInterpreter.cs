using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Function.Main;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Core;

public class JarfterInterpreter()
{
    [field: AllowNull, MaybeNull] public JarfterContext JarfterContext => field ??= new JarfterContext(this);
    
    public void Run(string[] input)
    {
        int length = input.Sum(str => str.Length);
        const string Leader = "block [";
        const string Tail = "]";
        length += Leader.Length + Tail.Length + input.Length * 3;

        using MutableString mutableString = new MutableString(length);
        mutableString.Append(Leader);
        foreach (string item in input)
        {
            mutableString.AppendFormattable('{');
            mutableString.Append(item);
            mutableString.AppendFormattable('}');
            mutableString.AppendFormattable(',');
        }
        mutableString.Append(Tail);
        Run(mutableString);
    }

    internal void RunConsole(ReadOnlySpan<char> input)
    {
        if (input.Length == 0 || input[0] is '#') return;
        JarfterFrame jarfterFrame = JarfterContext.CallingTree.Peek();
        IList<JarfterFunc> blockCodesContent = jarfterFrame.BlockCodes.Content;
        while (jarfterFrame.CurrIndex < blockCodesContent.Count)
        {
            JarfterFunc func = blockCodesContent[jarfterFrame.CurrIndex];
            ++jarfterFrame.CurrIndex;
            Run(func.FuncCode);
        }
    }
    
    public void Run(ReadOnlySpan<char> input)
    {
        if (input.Length == 0 || input[0] is '#') return;
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