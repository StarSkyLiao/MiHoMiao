namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBge(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"{"bge",-12}{labelName}";
}