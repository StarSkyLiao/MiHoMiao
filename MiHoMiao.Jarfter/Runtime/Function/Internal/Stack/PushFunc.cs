using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Stack;

[UsedImplicitly]
public class PushFunc : IJarfterFunc<PushFunc, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["push", "stack"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterObject arg0)
    {
        jarfterContext.CalculationStack.Push(arg0);
    }
    
}