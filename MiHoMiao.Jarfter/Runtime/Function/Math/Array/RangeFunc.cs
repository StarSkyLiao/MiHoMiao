using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Math.Method;

[UsedImplicitly]
public class RangeFunc : IJarfterFunc<RangeFunc, long, long>
{
    public string[] JarfterFuncName { get; } = ["range", "math"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, long arg0, long arg1)
    {
        jarfterContext.CalculationStack.Push(
            new JarfterArray<long>(Range(arg0, arg1).ToArray())
        );
    }
    
    private static IEnumerable<long> Range(long start, long tail)
    {
        for (long i = start; i < tail; i++) yield return i;
    }
    
}