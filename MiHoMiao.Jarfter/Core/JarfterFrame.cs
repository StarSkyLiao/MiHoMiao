using MiHoMiao.Jarfter.Core.Collection;

namespace MiHoMiao.Jarfter.Core;

public class JarfterFrame(JarfterArray<JarfterFunc> blockCodes)
{
    
    public JarfterArray<JarfterFunc> BlockCodes => blockCodes;

    public int CurrIndex;

}