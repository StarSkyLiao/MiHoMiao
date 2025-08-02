using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class FloorFunc : IJarfterFunc<FloorFunc, decimal>
{
    public string[] JarfterFuncName { get; } = ["floor", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0)
    {
        jarfterContext.CalculationStack.Push((long)System.Math.Floor(arg0));
    }
    
}