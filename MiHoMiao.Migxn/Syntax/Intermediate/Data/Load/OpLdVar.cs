namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

public class OpLdVar(ReadOnlyMemory<char> varName) : OpLdc
{
    public override string ToString() => $"ld.var    {varName}";
}