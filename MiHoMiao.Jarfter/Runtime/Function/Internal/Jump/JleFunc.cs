using JetBrains.Annotations;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Main;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Jump;

[UsedImplicitly]
public class JleFunc : IJarfterFunc<JleFunc, string>
{
    public string[] JarfterFuncName { get; } = ["jle", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        if (jarfterContext.CalculationStack.Count == 0) throw new EmptyCalculationStackException();
        int sign = (int)jarfterContext.CalculationStack.Pop()!;
        if (sign <= 0) GotoStmt.JarfterFuncImpl(jarfterContext, arg0);
    }
    
}