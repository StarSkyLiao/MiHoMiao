namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdNull : OpLdc
{
    public override string ToString() => $"ldc.null";
}