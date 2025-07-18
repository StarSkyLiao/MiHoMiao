using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Main;

public class RepeatStmt : IJarfterFunc<RepeatStmt, int, JarfterFunc>
{
    public string[] JarfterFuncName { get; } = ["repeat", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, int arg0, JarfterFunc arg1)
    {
        ReadOnlySpan<char> funcCode = arg1.FuncCode.AsSpan();
        for (int i = 0; i < arg0; i++) jarfterContext.JarfterInterpreter.Run(funcCode);
    }
    
}