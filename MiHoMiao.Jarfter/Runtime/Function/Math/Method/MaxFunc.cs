using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class MaxFunc : IJarfterFunc<MaxFunc, JarfterArray<decimal>>
{
    public string[] JarfterFuncName { get; } = ["max", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<decimal> arg0)
    {
        jarfterContext.CalculationStack.Push(arg0.Content.Max());
    }
}