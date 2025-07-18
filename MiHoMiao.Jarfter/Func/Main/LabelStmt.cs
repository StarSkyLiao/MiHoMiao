using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;
using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Func.Main;

[UsedImplicitly]
public class LabelStmt : IJarfterFunc<LabelStmt, string>
{
    public string[] JarfterFuncName { get; } = ["label", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        if (jarfterContext.CallingTree.Count == 0) throw new UnexceptedLabelException(arg0);
        var frame = jarfterContext.CallingTree.Peek();
        jarfterContext.JarfterLabelTable.DeclareVariable(arg0, (frame, frame.CurrIndex));
    }
    
}