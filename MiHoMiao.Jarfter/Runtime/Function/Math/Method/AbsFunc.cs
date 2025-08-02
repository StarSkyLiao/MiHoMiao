using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class AbsFunc : IJarfterFunc<AbsFunc, decimal>
{
    public string[] JarfterFuncName { get; } = ["abs", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0)
    {
        jarfterContext.CalculationStack.Push(System.Math.Abs(arg0));
    }
    
}