using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class LabelStmt : IJarfterFunc<LabelStmt, string>
{
    public string[] JarfterFuncName { get; } = ["label", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0)
    {
        if (jarfterContext.CallingTree.Count == 0) throw new UnexceptedLabelException(arg0);
        JarfterFrame frame = jarfterContext.CallingTree.Peek();
        jarfterContext.JarfterLabelTable.DeclareVariable(arg0, (frame, frame.CurrIndex));
    }
    
}