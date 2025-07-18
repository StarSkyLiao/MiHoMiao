using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Main;

public class ExecuteStmt : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["execute", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, bool arg0, JarfterFunc arg1, JarfterFunc? arg2)
    {
        if (arg0) jarfterContext.JarfterInterpreter.Run(arg1.FuncCode.AsSpan());
        else if (arg2 != null) jarfterContext.JarfterInterpreter.Run(arg2.FuncCode.AsSpan());
    }

    public void RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan)
    {
        JarfterFuncImpl(jarfterContext,
            IJarfterFunc.JarfterParse<bool>(jarfterContext, argSpan[0]),
            IJarfterFunc.JarfterParse<JarfterFunc>(jarfterContext, argSpan[1]),
            argSpan.Length > 2 ? IJarfterFunc.JarfterParse<JarfterFunc>(jarfterContext, argSpan[2]) : null
        );
    }
    
}