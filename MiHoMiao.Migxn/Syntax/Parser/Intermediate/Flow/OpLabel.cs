namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpLabel(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"label     {labelName}";
}