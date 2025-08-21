namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

internal class OpLdcI4(int value) : OpLdc
{
    public override string ToString() => $"ldc.i4    {value}";
}