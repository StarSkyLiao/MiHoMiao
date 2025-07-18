using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Compare;

[UsedImplicitly]
public class LessFunc : IJarfterFunc<LessFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["less", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 < arg1);
    }
}

[UsedImplicitly]
public class LessFuncFloat : IJarfterFunc<LessFuncFloat, double, double>
{
    public string[] JarfterFuncName { get; } = ["less.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 < arg1);
    }
}