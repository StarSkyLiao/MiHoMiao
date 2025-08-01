using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Trigonometric;

[UsedImplicitly]
public class TanFunc : IJarfterFunc<TanFunc, double>
{
    public string[] JarfterFuncName { get; } = ["tan", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0)
    {
        jarfterContext.CalculationStack.Push(System.Math.Tan(arg0));
    }
    
}