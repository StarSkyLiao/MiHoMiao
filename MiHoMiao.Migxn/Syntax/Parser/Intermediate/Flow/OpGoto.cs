namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpGoto(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.goto   {labelName}";
}