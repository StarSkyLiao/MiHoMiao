using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Function.Template;

[UsedImplicitly]
public interface IJarfterFunc<TSelf, in T0, in T1, in T2, in T3> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf, T0, T1, T2, T3>
    where T0 : ISpanParsable<T0>
    where T1 : ISpanParsable<T1>
    where T2 : ISpanParsable<T2>
    where T3 : ISpanParsable<T3>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        TSelf.JarfterFuncImpl(jarfterContext,
            JarfterParse<T0>(jarfterContext, input),
            JarfterParse<T1>(jarfterContext, input),
            JarfterParse<T2>(jarfterContext, input),
            JarfterParse<T3>(jarfterContext, input)
        );
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext, T0 arg0, T1 arg1, T2 arg2, T3 arg3);
}