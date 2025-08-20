namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpLabel(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"label     {labelName}";
}