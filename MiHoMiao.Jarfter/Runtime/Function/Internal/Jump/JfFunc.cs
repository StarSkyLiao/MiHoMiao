using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Main;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Jump;

[UsedImplicitly]
public class JfFunc : IJarfterFunc<JfFunc, bool, string>
{
    public string[] JarfterFuncName { get; } = ["jf", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, bool arg0, string arg1)
    {
        if (arg0) GotoStmt.JarfterFuncImpl(jarfterContext, arg1);
    }
    
}