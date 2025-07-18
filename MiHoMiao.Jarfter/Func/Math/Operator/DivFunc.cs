using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Math.Operator;

[UsedImplicitly]
public class DivFunc : IJarfterFunc<DivFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["div", "math"];
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 / arg1);
    }
    
}

[UsedImplicitly]
public class DivFuncFloat : IJarfterFunc<DivFuncFloat, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["div.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 / arg1);
    }
}