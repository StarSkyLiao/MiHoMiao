using MiHoMiao.Core.Serialization.IO;

namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdcStr(string value) : OpLdc
{
    public override string ToString() => $"{"ldc.string",-12}{value.Escape()}";
    public override string AsString() => value;
}