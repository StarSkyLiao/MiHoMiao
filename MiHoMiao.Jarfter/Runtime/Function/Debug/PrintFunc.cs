using JetBrains.Annotations;
using MiHoMiao.Core.Numerics.Values;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Debug;

[UsedImplicitly]
public class PrintFunc : IJarfterFunc<PrintFunc, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["print", "debug"];
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterObject arg0)
    {
        System.Console.WriteLine(arg0.ToString());
    }
}