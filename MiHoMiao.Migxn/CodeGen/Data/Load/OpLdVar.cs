namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdVar(string varName) : OpLdc
{
    public override string ToString() => $"{"ldc.var",-12}{varName}";
}