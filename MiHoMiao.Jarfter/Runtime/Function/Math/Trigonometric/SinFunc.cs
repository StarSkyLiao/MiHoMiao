using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Trigonometric;

[UsedImplicitly]
public class SinFunc : IJarfterFunc<SinFunc, double>
{
    public string[] JarfterFuncName { get; } = ["sin", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0)
    {
        jarfterContext.CalculationStack.Push(System.Math.Sin(arg0));
    }
    
}