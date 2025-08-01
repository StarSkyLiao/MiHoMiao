using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Array;

[UsedImplicitly]
public class RangeFunc : IJarfterFunc<RangeFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["range", "array"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(
            new JarfterArray<decimal>(Range(arg0, arg1).ToArray())
        );
    }
    
    private static IEnumerable<decimal> Range(long start, long tail)
    {
        for (long i = start; i < tail; i++) yield return i;
    }
    
}