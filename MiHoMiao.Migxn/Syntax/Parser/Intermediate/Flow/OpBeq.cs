namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBeq(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"beq       {labelName}";
}