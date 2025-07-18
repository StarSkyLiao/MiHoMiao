using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Function.Template;

[UsedImplicitly]
public interface IJarfterFunc<TSelf> : IJarfterFunc where TSelf : IJarfterFunc<TSelf>
{
    void IJarfterFunc.RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        TSelf.JarfterFuncImpl(jarfterContext);
    }
    
    protected static abstract void JarfterFuncImpl(JarfterContext jarfterContext);
}