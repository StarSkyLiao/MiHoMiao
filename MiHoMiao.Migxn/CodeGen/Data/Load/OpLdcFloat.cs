namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdcFloat(double value) : OpLdc
{
    public override string ToString() => $"{"ldc.float",-12}{value}";
}