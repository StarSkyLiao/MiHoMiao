using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Math.Compare;

[UsedImplicitly]
public class LessEqualFunc : IJarfterFunc<LessEqualFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["lese", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 <= arg1);
    }
}

[UsedImplicitly]
public class LessEqualFuncFloat : IJarfterFunc<LessEqualFuncFloat, double, double>
{
    public string[] JarfterFuncName { get; } = ["lese.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 <= arg1);
    }
}