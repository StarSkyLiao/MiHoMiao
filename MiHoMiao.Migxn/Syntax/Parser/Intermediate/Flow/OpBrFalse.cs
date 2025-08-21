namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBrFalse(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.false  {labelName}";
}