using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Compare;

[UsedImplicitly]
public class GreaterEqualFunc : IJarfterFunc<GreaterEqualFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["gre", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 >= arg1);
    }
}

[UsedImplicitly]
public class GreaterEqualFuncFloat : IJarfterFunc<GreaterEqualFuncFloat, double, double>
{
    public string[] JarfterFuncName { get; } = ["gre.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 >= arg1);
    }
}