using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpSwitch(ReadOnlyMemory<char>[] labelName) : MigxnOpCode
{
    public override string ToString() => $"switch     {labelName.GenericViewer("{", "}")}";
}