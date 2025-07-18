using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Compare;

[UsedImplicitly]
public class GreaterFunc : IJarfterFunc<GreaterFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["grt", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 > arg1);
    }
}

[UsedImplicitly]
public class GreaterFuncFloat : IJarfterFunc<GreaterFuncFloat, double, double>
{
    public string[] JarfterFuncName { get; } = ["grt.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 > arg1);
    }
}