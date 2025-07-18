using JetBrains.Annotations;

namespace MiHoMiao.Jarfter.Core.Func;

[UsedImplicitly]
public interface IJarfterFunc<TSelf> : IJarfterFunc
    where TSelf : IJarfterFunc<TSelf>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan)
    {
        TSelf.JarfterFuncImpl(jarfterContext);
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext);
}