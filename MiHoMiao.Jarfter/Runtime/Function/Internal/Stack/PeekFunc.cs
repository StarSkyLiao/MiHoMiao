using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Stack;

[UsedImplicitly]
public class PeekFunc : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["peek", "stack"];
    
    public void RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        jarfterContext.CalculationStack.Push(jarfterContext.CalculationStack.Peek());
    }
    
}