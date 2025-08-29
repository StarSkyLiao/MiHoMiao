namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdVar(string varName) : OpLdc
{
    public override string ToString() => $"ld.var    {varName}";
}