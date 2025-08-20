namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

public class OpLdcR8(double value) : OpLdc
{
    public override string ToString() => $"ldc.r8    {value}";
}