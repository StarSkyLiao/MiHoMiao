namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBrTrue(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.true   {labelName}";
}