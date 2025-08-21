namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBgt(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bgt       {labelName}";
}