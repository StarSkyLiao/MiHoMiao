using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class LetStmt : IJarfterFunc<LetStmt, string, JarfterObject>
{
    public string[] JarfterFuncName { get; } = ["let", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, JarfterObject arg1)
    {
        jarfterContext.JarfterSymbolTable.StoreVariable(arg0, arg1);
    }
    
}