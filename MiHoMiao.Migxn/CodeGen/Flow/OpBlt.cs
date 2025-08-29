namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBlt(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"{"blt",-12}{labelName}";
}