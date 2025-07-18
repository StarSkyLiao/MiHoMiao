using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class VarStmt : IJarfterFunc<VarStmt, string, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["var", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, JarfterObject arg1)
    {
        jarfterContext.JarfterSymbolTable.DeclareVariable(arg0, arg1);
    }
    
}