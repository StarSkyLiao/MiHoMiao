using JetBrains.Annotations;
using MiHoMiao.Core.Numerics.Values;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Math.Random;

[UsedImplicitly]
public class RandomIntFunc : IJarfterFunc<RandomIntFunc, int, int>
{
    public string[] JarfterFuncName { get; } = ["random.int", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, int arg0, int arg1)
    {
        jarfterContext.CalculationStack.Push(Randomization.Range(arg0, arg1));
    }
}