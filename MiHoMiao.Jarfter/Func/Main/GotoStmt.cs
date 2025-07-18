using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Func.Main;

[UsedImplicitly]
public class GotoStmt : IJarfterFunc<GotoStmt, string>
{
    public string[] JarfterFuncName { get; } = ["goto", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        if (jarfterContext.CallingTree.Count == 0) throw new UnexceptedLabelException(arg0);
        (JarfterFrame? jarfterFrame, int? index) = jarfterContext.JarfterLabelTable.LoadVariable(arg0);
        if (jarfterFrame is null) throw new UnexceptedLabelException(arg0);
        var frame = jarfterContext.CallingTree.Peek();
        if (jarfterFrame != frame) throw new InvalidCallingTreeException();
        frame.CurrIndex = index.Value;
    }
    
}