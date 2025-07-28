using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class BlockStmt : IJarfterFunc<BlockStmt, JarfterArray<JarfterFunc>>
{
    public string[] JarfterFuncName { get; } = ["block", "main"];

    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<JarfterFunc> arg0)
    {
        JarfterFrame currFrame = new JarfterFrame(arg0);
        jarfterContext.PushStack(currFrame);
        if (RunBlockBody(jarfterContext, arg0, currFrame)) jarfterContext.PopStack();
    }

    /// <summary>
    /// 返回是否需要 Pop 栈帧.
    /// 如果为 false, 则表示栈帧已被外部修改, 无需重复处理
    /// </summary>
    internal static bool RunBlockBody(JarfterContext jarfterContext, JarfterArray<JarfterFunc> arg0,
        JarfterFrame currFrame)
    {
        for (currFrame.CurrIndex = 0; currFrame.CurrIndex < arg0.Content.Count; ++currFrame.CurrIndex)
        {
            JarfterFunc jarfterFunc = arg0.Content[currFrame.CurrIndex];
            jarfterContext.JarfterInterpreter.Run(jarfterFunc.FuncCode);
            if (!jarfterContext.CallingTree.TryPeek(out JarfterFrame? frame)) throw new InvalidCallingTreeException();
            if (frame != currFrame) return false;
        }
        return true;
    }
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, JarfterArray<string> arg0)
    {
        JarfterFunc[] funcArray = new JarfterFunc[arg0.Content.Count];
        for (int index = 0; index < arg0.Content.Count; index++)
        {
            string item = arg0.Content[index];
            funcArray[index] = new JarfterFunc(item.StartsWith('{') ? item[1..^1] : item);
        }
        
        JarfterArray<JarfterFunc> array = new JarfterArray<JarfterFunc>(funcArray);
        JarfterFrame currFrame = new JarfterFrame(array);
        jarfterContext.PushStack(currFrame);
        if (RunBlockBody(jarfterContext, array, currFrame)) jarfterContext.PopStack();
    }
    
}