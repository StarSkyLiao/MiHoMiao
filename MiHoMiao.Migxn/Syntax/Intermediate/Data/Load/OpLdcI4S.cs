namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

public class OpLdcI4S(sbyte value) : OpLdc
{
    public override string ToString() => $"ldc.i4.s  {value}";
}