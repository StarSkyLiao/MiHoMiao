using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class RoundFunc : IJarfterFunc<RoundFunc, decimal>
{
    public string[] JarfterFuncName { get; } = ["round", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0)
    {
        jarfterContext.CalculationStack.Push((long)System.Math.Round(arg0));
    }
}