using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Operator;

[UsedImplicitly]
public class MulFunc : IJarfterFunc<MulFunc, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["mul", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 * arg1);
    }
    
}

[UsedImplicitly]
public class MulFuncList : IJarfterFunc<MulFuncList, JarfterArray<decimal>>
{
    public string[] JarfterFuncName { get; } = ["mul.list", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<decimal> arg1)
    {
        decimal result = 1;
        foreach (decimal item in arg1.Content) result *= item;
        jarfterContext.CalculationStack.Push(result);
    }

}