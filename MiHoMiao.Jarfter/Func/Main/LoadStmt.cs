using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Main;

public class LoadStmt : IJarfterFunc<LoadStmt, string>
{
    public string[] JarfterFuncName { get; } = ["load", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        jarfterContext.CalculationStack.Push(jarfterContext.JarfterSymbolTable.LoadVariable(arg0));
    }
    
}