using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Trigonometric;

[UsedImplicitly]
public class CosFunc : IJarfterFunc<CosFunc, double>
{
    public string[] JarfterFuncName { get; } = ["cos", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0)
    {
        jarfterContext.CalculationStack.Push(System.Math.Cos(arg0));
    }
    
}