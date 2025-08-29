namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdVar(ReadOnlyMemory<char> varName) : OpLdc
{
    public override string ToString() => $"ld.var    {varName}";
}