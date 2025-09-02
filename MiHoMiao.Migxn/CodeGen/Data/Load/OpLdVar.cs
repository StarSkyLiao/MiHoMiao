namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdVar(string varName) : MigxnOpCode
{
    public override string ToString() => $"{"ldc.var",-12}{varName}";
}