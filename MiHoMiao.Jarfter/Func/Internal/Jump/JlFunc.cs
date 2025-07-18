using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Func.Main;

namespace MiHoMiao.Jarfter.Func.Internal.Jump;

[UsedImplicitly]
public class JlFunc : IJarfterFunc<JlFunc, string>
{
    public string[] JarfterFuncName { get; } = ["jl", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        if (jarfterContext.CalculationStack.Count == 0) throw new EmptyCalculationStackException();
        int sign = (int)jarfterContext.CalculationStack.Pop()!;
        if (sign < 0) GotoStmt.JarfterFuncImpl(jarfterContext, arg0);
    }
    
}