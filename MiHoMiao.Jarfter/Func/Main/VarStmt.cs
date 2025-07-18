using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Main;

public class VarStmt : IJarfterFunc<VarStmt, string, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["var", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, JarfterObject arg1)
    {
        jarfterContext.JarfterSymbolTable.DeclareVariable(arg0, arg1);
    }
    
}