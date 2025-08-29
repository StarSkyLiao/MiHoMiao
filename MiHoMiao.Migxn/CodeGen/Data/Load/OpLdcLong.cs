namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdcLong(long value) : OpLdc
{
    public override string ToString() => $"{"ldc.int64",-12}{value}";
}