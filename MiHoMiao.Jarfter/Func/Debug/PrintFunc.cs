using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Debug;

[UsedImplicitly]
public class PrintFunc : IJarfterFunc<PrintFunc, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["print", "debug"];
    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterObject arg0) => Console.Write(arg0.RawString);
}