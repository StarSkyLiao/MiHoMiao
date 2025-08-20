namespace MiHoMiao.Migxn.Syntax.Intermediate.Data.Store;

public class OpStVar(ReadOnlyMemory<char> varName) : MigxnOpCode
{
    public override string ToString() => $"st.var    {varName}";
}