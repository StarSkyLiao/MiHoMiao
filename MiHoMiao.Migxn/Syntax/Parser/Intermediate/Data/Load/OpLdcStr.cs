namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

internal class OpLdcStr(string value) : OpLdc
{
    public override string ToString() => $"ldc.str   {value}";
}