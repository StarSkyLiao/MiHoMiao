using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Function.Template;

[UsedImplicitly]
public interface IJarfterFunc<TSelf, in T0> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf, T0>
    where T0 : ISpanParsable<T0>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        TSelf.JarfterFuncImpl(jarfterContext,
            JarfterParse<T0>(jarfterContext, input)
        );
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext, T0 arg0);
}