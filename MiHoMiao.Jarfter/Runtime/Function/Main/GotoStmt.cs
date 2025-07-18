using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class GotoStmt : IJarfterFunc<GotoStmt, string>
{
    public string[] JarfterFuncName { get; } = ["goto", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        if (jarfterContext.CallingTree.Count == 0) throw new UnexceptedLabelException(arg0);
        (JarfterFrame? jarfterFrame, int? index) = jarfterContext.JarfterLabelTable.LoadVariable(arg0);
        if (jarfterFrame is null) throw new UnexceptedLabelException(arg0);

        while (jarfterContext.CallingTree.Count > 0)
        {
            if (!jarfterContext.CallingTree.TryPeek(out JarfterFrame? frame))
                throw new InvalidCallingTreeException();
            if (frame == jarfterFrame) break;
            jarfterContext.CallingTree.Pop();
        }
        
        jarfterFrame.CurrIndex = index.Value;
    }
    
}