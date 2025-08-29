namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBgt(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"{"bgt",-12}{labelName}";
}