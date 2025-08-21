namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

internal class OpLdcR8(double value) : OpLdc
{
    public override string ToString() => $"ldc.r8    {value}";
}