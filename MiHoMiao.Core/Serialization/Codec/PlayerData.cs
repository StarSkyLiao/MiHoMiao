using System.Text;
using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Core.Serialization.Codec;

[AutoComplexCodec]
internal partial record PlayerData(Guid Id, int Level, string Name, int[] Items)
{
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("PlayerData");
        builder.Append(" { ");
        if (PrintMembers(builder)) builder.Append(' ');
        builder.Append($"Items={Items.GenericViewer()}");
        builder.Append('}');
        return builder.ToString();
    }
    
}

[AutoComplexCodec]
internal partial record PlayerGroup(PlayerData PlayerData, int Count, string[] Friends, string[][] CrossArray) 
{
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("PlayerGroup");
        builder.Append(" { ");
        if (PrintMembers(builder)) builder.Append(' ');
        builder.Append($"Friends={Friends.GenericViewer()}");
        builder.Append($"CrossArray={CrossArray.GenericViewer()}");
        builder.Append('}');
        return builder.ToString();
    }
    
}

[AutoComplexCodec]
internal partial record struct CrossArray(int[][] Values);