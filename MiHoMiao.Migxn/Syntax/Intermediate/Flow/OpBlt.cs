namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBlt(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"blt       {labelName}";
}