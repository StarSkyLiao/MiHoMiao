namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBeq(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"beq",-12}{labelName}";
}