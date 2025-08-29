namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBeq(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"{"beq",-12}{labelName}";
}