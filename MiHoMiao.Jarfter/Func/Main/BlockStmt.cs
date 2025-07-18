using JetBrains.Annotations;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Collection;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Func.Main;

[UsedImplicitly]
public class BlockStmt : IJarfterFunc<BlockStmt, JarfterArray<JarfterFunc>>
{
    public string[] JarfterFuncName { get; } = ["block", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<JarfterFunc> arg0)
    {
        JarfterFrame currFrame = new JarfterFrame(arg0);
        jarfterContext.PushStack(currFrame);
        for (currFrame.CurrIndex = 0; currFrame.CurrIndex < arg0.Items.Length; ++currFrame.CurrIndex)
        {
            JarfterFunc jarfterFunc = arg0.Items[currFrame.CurrIndex];
            jarfterContext.JarfterInterpreter.Run(jarfterFunc.FuncCode);
        }
        jarfterContext.PopStack();
    }
    
}