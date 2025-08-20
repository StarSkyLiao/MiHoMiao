namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

public class OpLdcStr(string value) : OpLdc
{
    public override string ToString() => $"ldc.str   {value}";
}