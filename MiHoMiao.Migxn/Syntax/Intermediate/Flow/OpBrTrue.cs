namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBrTrue(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.true   {labelName}";
}