namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBrFalse(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.false  {labelName}";
}