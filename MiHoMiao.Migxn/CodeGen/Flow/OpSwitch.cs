using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpSwitch(ReadOnlyMemory<char>[] labelNames) : MigxnOpCode
{
    public override string ToString() => $"{"switch",-12}{labelNames.GenericViewer("{", "}")}";
}