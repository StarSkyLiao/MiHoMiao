using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpSwitch(string[] labelNames) : MigxnOpCode
{
    public override string ToString() => $"{"switch",-12}{labelNames.GenericViewer("{", "}")}";
}