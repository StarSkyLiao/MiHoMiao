namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBlt(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"blt       {labelName}";
}