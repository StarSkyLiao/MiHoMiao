namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpLabel(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"label",-12}{labelName}";
}