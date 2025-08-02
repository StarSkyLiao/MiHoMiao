using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class SignFunc : IJarfterFunc<SignFunc, decimal>
{
    public string[] JarfterFuncName { get; } = ["sign", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, decimal arg0)
    {
        jarfterContext.CalculationStack.Push(System.Math.Sign(arg0));
    }
    
}