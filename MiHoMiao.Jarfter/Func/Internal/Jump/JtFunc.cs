using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Func.Main;

namespace MiHoMiao.Jarfter.Func.Internal.Jump;

[UsedImplicitly]
public class JtFunc : IJarfterFunc<JtFunc, bool, string>
{
    public string[] JarfterFuncName { get; } = ["jt", "main"];
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, bool arg0, string arg1)
    {
        if (arg0) GotoStmt.JarfterFuncImpl(jarfterContext, arg1);
    }
    
}