using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Main;

public sealed class RepeatStmt : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["repeat", "main"];
    
    public void RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        JarfterFuncImpl(jarfterContext,
            IJarfterFunc.JarfterParse<uint>(jarfterContext, input),
            IJarfterFunc.JarfterParse<JarfterArray<JarfterFunc>>(jarfterContext, input),
            jarfterContext.ParsingIndex < input.Length
                ? IJarfterFunc.JarfterParse<JarfterWord>(jarfterContext, input).Content
                : "i"
        );
    }

    private static void JarfterFuncImpl(JarfterContext jarfterContext, uint arg0, JarfterArray<JarfterFunc> arg1,
        string indexName)
    {
        JarfterFrame currFrame = new JarfterFrame(arg1);
        jarfterContext.PushStack(currFrame);
        IndexNumber index = new IndexNumber(0);
        bool isFrameInvalid = false;
        jarfterContext.JarfterSymbolTable.DeclareVariable(indexName, new JarfterUnit(index));
        while (index.Value < arg0)
        {
            isFrameInvalid |= BlockStmt.RunBlockBody(jarfterContext, arg1, currFrame);
            ++index.Value;
        }

        if (isFrameInvalid) jarfterContext.PopStack();
    }

    private class IndexNumber(uint value)
    {
        public uint Value = value;

        public override string ToString() => Value.ToString();
    }

}