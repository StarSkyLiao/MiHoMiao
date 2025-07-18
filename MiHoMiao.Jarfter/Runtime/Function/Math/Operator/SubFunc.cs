using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Operator;

[UsedImplicitly]
public class SubFunc : IJarfterFunc<SubFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["sub", "math"];
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 - arg1);
    }

}

[UsedImplicitly]
public class SubFuncFloat : IJarfterFunc<SubFuncFloat, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["sub.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 - arg1);
    }
}