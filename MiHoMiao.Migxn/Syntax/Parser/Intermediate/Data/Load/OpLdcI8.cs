namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

internal class OpLdcI8(long value) : OpLdc
{
    public override string ToString() => $"ldc.i8    {value}";
}