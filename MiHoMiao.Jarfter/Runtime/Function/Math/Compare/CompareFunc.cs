using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Compare;

[UsedImplicitly]
public class CompareFunc : IJarfterFunc<CompareFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["cmp", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(System.Math.Sign(arg0 - arg1));
    }
}

[UsedImplicitly]
public class CompareFuncFloat : IJarfterFunc<CompareFuncFloat, double, double>
{
    public string[] JarfterFuncName { get; } = ["cmp.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(System.Math.Sign(arg0 - arg1));
    }
}