using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Compare;

[UsedImplicitly]
public class NotEqualFunc : IJarfterFunc<NotEqualFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["neq", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 != arg1);
    }
}

[UsedImplicitly]
public class NotEqualFuncFloat : IJarfterFunc<NotEqualFuncFloat, double, double>
{
    public string[] JarfterFuncName { get; } = ["neq.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(System.Math.Abs(arg0 - arg1) >= 0.001);
    }
}