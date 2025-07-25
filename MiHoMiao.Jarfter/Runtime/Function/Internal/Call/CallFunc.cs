using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Call;

[UsedImplicitly]
public class CallFunc : IJarfterFunc<CallFunc, string, JarfterArray<string>>
{
    public string[] JarfterFuncName { get; } = ["call", "internal"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, JarfterArray<string> arg1)
    {
        jarfterContext.CalculationStack.Push(CallHelper.Invoke(jarfterContext, arg0, arg1.Content));
    }

}