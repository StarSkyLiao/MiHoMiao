using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpSwitch(ReadOnlyMemory<char>[] labelName) : MigxnOpCode
{
    public override string ToString() => $"switch     {labelName.GenericViewer("{", "}")}";
}