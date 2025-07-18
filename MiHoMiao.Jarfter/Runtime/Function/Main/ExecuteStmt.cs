using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class ExecuteStmt : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["execute", "main"];
    
    public void RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        ref int index = ref jarfterContext.ParsingIndex;
        var arg0 = IJarfterFunc.JarfterParse<bool>(jarfterContext, input);
        var arg1 = IJarfterFunc.JarfterParse<JarfterFunc>(jarfterContext, input);
        while (index < input.Length - 1 && char.IsWhiteSpace(input[index])) ++index;
        if (!IsElse(input, index)) JarfterFuncImpl(jarfterContext, arg0, arg1, null, null);
        else
            JarfterFuncImpl(jarfterContext, arg0, arg1,
                IJarfterFunc.JarfterParse<JarfterWord>(jarfterContext, input),
                IJarfterFunc.JarfterParse<JarfterFunc>(jarfterContext, input)
            );
    }

    public static void JarfterFuncImpl(JarfterContext jarfterContext, bool arg0, JarfterFunc arg1, 
        JarfterWord? arg2, JarfterFunc? arg3)
    {
        if (arg0)
        {
            jarfterContext.JarfterInterpreter.Run(arg1.FuncCode.AsSpan());
            return;
        }
        if (arg2 is null) return;
        if (arg2.Content != "else" || arg3 == null) throw new FunctionNotFoundException(arg2.Content);
        jarfterContext.JarfterInterpreter.Run(arg3.FuncCode.AsSpan());
    }
    
    private static bool IsElse(ReadOnlySpan<char> span, int index)
    {
        if (index < 0 || index + 4 > span.Length) return false;
        return span.Slice(index, 4).SequenceEqual("else".AsSpan());
    }
    
}