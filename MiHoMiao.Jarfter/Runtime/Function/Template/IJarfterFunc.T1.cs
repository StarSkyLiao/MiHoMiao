using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Function.Template;

[UsedImplicitly]
public interface IJarfterFunc<TSelf, in T0, in T1> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf, T0, T1>
    where T0 : ISpanParsable<T0>
    where T1 : ISpanParsable<T1>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        TSelf.JarfterFuncImpl(jarfterContext,
            JarfterParse<T0>(jarfterContext, input),
            JarfterParse<T1>(jarfterContext, input)
        );
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext, T0 arg0, T1 arg1);
}