using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Internal.Stack;

[UsedImplicitly]
public class PopFunc : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["pop", "stack"];
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        jarfterContext.JarfterSymbolTable.StoreVariable(arg0, jarfterContext.CalculationStack.Pop());
    }

    public void RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan)
    {
        JarfterFuncImpl(jarfterContext,
            argSpan.Length > 0 ? IJarfterFunc.JarfterParse<string>(jarfterContext, argSpan[0]) : "_"
        );
    }
    
}