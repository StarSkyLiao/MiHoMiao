using JetBrains.Annotations;
using MiHoMiao.Core.Numerics.Values;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Math.Random;

[UsedImplicitly]
public class RandomFloatFunc : IJarfterFunc<RandomFloatFunc, double, double>
{
    public string[] JarfterFuncName { get; } = ["random.float", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, double arg0, double arg1)
    {
        jarfterContext.CalculationStack.Push(Randomization.Range(arg0, arg1));
    }
}