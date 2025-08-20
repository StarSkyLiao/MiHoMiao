namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBeq(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"beq       {labelName}";
}