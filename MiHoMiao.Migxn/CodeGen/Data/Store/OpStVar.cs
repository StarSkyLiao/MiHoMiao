namespace MiHoMiao.Migxn.CodeGen.Data.Store;

internal class OpStVar(string varName) : MigxnOpCode
{
    public override string ToString() => $"{"st.var",-12}{varName}";
}