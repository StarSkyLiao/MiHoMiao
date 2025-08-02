using JetBrains.Annotations;
using MiHoMiao.Core.Numerics.Values;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class ClampFunc : IJarfterFunc<ClampFunc, decimal, decimal, decimal>
{
    public string[] JarfterFuncName { get; } = ["clamp", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0, decimal arg1, decimal arg2)
    {
        jarfterContext.CalculationStack.Push(arg0.Clamp(arg1, arg2));
    }
    
}