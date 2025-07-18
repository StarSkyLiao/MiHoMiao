using JetBrains.Annotations;

namespace MiHoMiao.Jarfter.Core.Func;

[UsedImplicitly]
public interface IJarfterFunc<TSelf, in T0, in T1, in T2, in T3> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf, T0, T1, T2, T3>
    where T0 : ISpanParsable<T0>
    where T1 : ISpanParsable<T1>
    where T2 : ISpanParsable<T2>
    where T3 : ISpanParsable<T3>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan)
    {
        TSelf.JarfterFuncImpl(jarfterContext,
            JarfterParse<T0>(jarfterContext, argSpan[0]),
            JarfterParse<T1>(jarfterContext, argSpan[1]),
            JarfterParse<T2>(jarfterContext, argSpan[2]),
            JarfterParse<T3>(jarfterContext, argSpan[3])
        );
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
}