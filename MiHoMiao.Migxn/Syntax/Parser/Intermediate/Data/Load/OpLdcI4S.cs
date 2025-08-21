namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

internal class OpLdcI4S(sbyte value) : OpLdc
{
    public override string ToString() => $"ldc.i4.s  {value}";
}