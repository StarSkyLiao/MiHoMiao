using JetBrains.Annotations;

namespace MiHoMiao.Jarfter.Core.Func;

[UsedImplicitly]
public interface IJarfterFunc<TSelf, in T0, in T1> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf, T0, T1>
    where T0 : ISpanParsable<T0>
    where T1 : ISpanParsable<T1>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan)
    {
        TSelf.JarfterFuncImpl(jarfterContext,
            JarfterParse<T0>(jarfterContext, argSpan[0]),
            JarfterParse<T1>(jarfterContext, argSpan[1])
        );
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext, T0 arg0, T1 arg1);
}