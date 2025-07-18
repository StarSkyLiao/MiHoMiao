using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Internal.Stack;

[UsedImplicitly]
public class PushFunc : IJarfterFunc<PushFunc, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["push", "stack"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterObject arg0)
    {
        jarfterContext.CalculationStack.Push(arg0);
    }
    
}