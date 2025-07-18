using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Main;

public class LetStmt : IJarfterFunc<LetStmt, string, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["let", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, JarfterObject arg1)
    {
        jarfterContext.JarfterSymbolTable.StoreVariable(arg0, arg1);
    }
    
}