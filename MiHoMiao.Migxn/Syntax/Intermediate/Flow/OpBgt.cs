namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBgt(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bgt       {labelName}";
}