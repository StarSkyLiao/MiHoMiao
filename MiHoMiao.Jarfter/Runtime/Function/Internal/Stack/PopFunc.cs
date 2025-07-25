using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Stack;

[UsedImplicitly]
public class PopFunc : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["pop", "stack"];
    
    public void RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        jarfterContext.JarfterSymbolTable.StoreVariable(
            input.Length > 0 ? IJarfterFunc.JarfterParse<string>(jarfterContext, input) : "_",
            new JarfterUnit(jarfterContext.CalculationStack.Pop()!)
        );
    }
    
}