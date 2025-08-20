namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

public class OpLdcI8(long value) : OpLdc
{
    public override string ToString() => $"ldc.i8    {value}";
}