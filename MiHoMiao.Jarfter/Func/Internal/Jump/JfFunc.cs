using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Func.Main;

namespace MiHoMiao.Jarfter.Func.Internal.Jump;

[UsedImplicitly]
public class JfFunc : IJarfterFunc<JfFunc, bool, string>
{
    public string[] JarfterFuncName { get; } = ["jf", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, bool arg0, string arg1)
    {
        if (arg0) GotoStmt.JarfterFuncImpl(jarfterContext, arg1);
    }
    
}