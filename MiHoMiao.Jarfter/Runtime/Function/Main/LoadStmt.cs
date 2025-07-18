
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class LoadStmt : IJarfterFunc<LoadStmt, string>
{
    public string[] JarfterFuncName { get; } = ["load", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        jarfterContext.CalculationStack.Push(jarfterContext.JarfterSymbolTable.LoadVariable(arg0));
    }
    
}