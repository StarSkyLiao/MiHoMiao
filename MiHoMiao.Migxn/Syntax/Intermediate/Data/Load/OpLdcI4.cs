namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

public class OpLdcI4(int value) : OpLdc
{
    public override string ToString() => $"ldc.i4    {value}";
}