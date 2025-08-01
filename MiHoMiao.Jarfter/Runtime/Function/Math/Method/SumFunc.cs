using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class SumFunc : IJarfterFunc<SumFunc, JarfterArray<decimal>>
{
    public string[] JarfterFuncName { get; } = ["sum", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<decimal> arg0)
    {
        jarfterContext.CalculationStack.Push(arg0.Content.Sum());
    }
}