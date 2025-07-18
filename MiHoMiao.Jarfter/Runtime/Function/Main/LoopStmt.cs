using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class LoopStmt : IJarfterFunc<LoopStmt, uint, JarfterFunc>
{
    public string[] JarfterFuncName { get; } = ["loop", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, uint arg0, JarfterFunc arg1)
    {
        ReadOnlySpan<char> funcCode = arg1.FuncCode.AsSpan();
        for (int i = 0; i < arg0; i++) jarfterContext.JarfterInterpreter.Run(funcCode);
    }
    
}