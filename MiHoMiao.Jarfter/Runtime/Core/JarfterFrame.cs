using MiHoMiao.Jarfter.Runtime.Collection;

namespace MiHoMiao.Jarfter.Runtime.Core;

public class JarfterFrame(JarfterArray<JarfterFunc> blockCodes)
{
    
    public JarfterArray<JarfterFunc> BlockCodes => blockCodes;

    public int CurrIndex;

}