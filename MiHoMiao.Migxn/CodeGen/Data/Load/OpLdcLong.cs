namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdcLong(long value) : OpLdc
{
    public readonly long Value = value;
    public override string ToString() => $"{"ldc.int64",-12}{Value}";
    public override string AsString() => Value.ToString();
}