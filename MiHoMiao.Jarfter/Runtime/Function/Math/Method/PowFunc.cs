using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class PowFunc : IJarfterFunc<PowFunc, double, double>
{
    public string[] JarfterFuncName { get; } = ["pow", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(System.Math.Pow(arg0, arg1));
    }
    
}