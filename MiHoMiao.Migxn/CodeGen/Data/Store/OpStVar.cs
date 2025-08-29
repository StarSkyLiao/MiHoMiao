namespace MiHoMiao.Migxn.CodeGen.Data.Store;

internal class OpStVar(ReadOnlyMemory<char> varName) : MigxnOpCode
{
    public override string ToString() => $"st.var    {varName}";
}