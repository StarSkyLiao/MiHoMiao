using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Operator;

[UsedImplicitly]
public class AddFunc : IJarfterFunc<AddFunc, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["add", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
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
public class AddFuncList : IJarfterFunc<AddFuncList, JarfterArray<decimal>>
{
    public string[] JarfterFuncName { get; } = ["add.list", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<decimal> arg1)
    {
        jarfterContext.CalculationStack.Push(arg1.Content.Sum());
    }

}
