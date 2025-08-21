namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBge(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bge       {labelName}";
}