using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Math.Operator;

[UsedImplicitly]
public class ModFunc : IJarfterFunc<ModFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["mod", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 % arg1);
    }
    
}

[UsedImplicitly]
public class ModFuncFloat : IJarfterFunc<ModFuncFloat, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["mod.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1)
    {
        jarfterContext.CalculationStack.Push(arg0 % arg1);
    }
}