using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Math.Operator;

[UsedImplicitly]
public class AddFunc : IJarfterFunc<AddFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["add", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 + arg1);
    }
}

[UsedImplicitly]
public class AddFuncString : IJarfterFunc<AddFuncString, string, string>
{
    public string[] JarfterFuncName { get; } = ["add.string", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, string arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 + arg1);
    }
}

[UsedImplicitly]
public class AddFuncFloat : IJarfterFunc<AddFuncFloat, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["add.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 + arg1);
    }
}

[UsedImplicitly]
public class AddFuncList : IJarfterFunc<AddFuncList, JarfterArray<long>>
{
    public string[] JarfterFuncName { get; } = ["add.list", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<long> arg1)
    {
        long result = 0;
        foreach (long item in arg1.Items) result += item;
        jarfterContext.CalculationStack.Push(result);
    }

}

[UsedImplicitly]
public class AddFuncFloatList : IJarfterFunc<AddFuncFloatList, JarfterArray<decimal>>
{
    public string[] JarfterFuncName { get; } = ["add.float.list", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<decimal> arg1)
    {
        long result = 0;
        foreach (long item in arg1.Items) result += item;
        jarfterContext.CalculationStack.Push(result);
    }

}