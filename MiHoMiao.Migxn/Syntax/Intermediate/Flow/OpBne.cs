namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBne(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bne       {labelName}";
}