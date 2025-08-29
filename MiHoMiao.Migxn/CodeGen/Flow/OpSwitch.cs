using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpSwitch(ReadOnlyMemory<char>[] labelName) : MigxnOpCode
{
    public override string ToString() => $"switch     {labelName.GenericViewer("{", "}")}";
}