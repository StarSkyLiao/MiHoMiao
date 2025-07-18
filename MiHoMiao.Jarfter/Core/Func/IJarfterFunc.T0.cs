using JetBrains.Annotations;

namespace MiHoMiao.Jarfter.Core.Func;

[UsedImplicitly]
public interface IJarfterFunc<TSelf, in T0> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf, T0>
    where T0 : ISpanParsable<T0>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan)
    {
        TSelf.JarfterFuncImpl(jarfterContext,
            JarfterParse<T0>(jarfterContext, argSpan[0])
        );
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext, T0 arg0);
}