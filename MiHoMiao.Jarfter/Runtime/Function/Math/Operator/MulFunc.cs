using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Operator;

[UsedImplicitly]
public class MulFunc : IJarfterFunc<MulFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["mul", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 * arg1);
    }
    
}

[UsedImplicitly]
public class MulFuncFloat : IJarfterFunc<MulFuncFloat, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["mul.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 * arg1);
    }
}

[UsedImplicitly]
public class MulFuncList : IJarfterFunc<MulFuncList, JarfterArray<long>>
{
    public string[] JarfterFuncName { get; } = ["mul.list", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<long> arg1)
    {
        long result = 1;
        foreach (long item in arg1.Content) result *= item;
        jarfterContext.CalculationStack.Push(result);
    }

}

[UsedImplicitly]
public class MulFuncFloatList : IJarfterFunc<MulFuncFloatList, JarfterArray<decimal>>
{
    public string[] JarfterFuncName { get; } = ["mul.float.list", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<decimal> arg1)
    {
        long result = 1;
        foreach (long item in arg1.Content) result *= item;
        jarfterContext.CalculationStack.Push(result);
    }

}