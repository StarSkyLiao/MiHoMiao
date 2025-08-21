namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Store;

internal class OpStVar(ReadOnlyMemory<char> varName) : MigxnOpCode
{
    public override string ToString() => $"st.var    {varName}";
}