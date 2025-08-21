namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBne(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bne       {labelName}";
}