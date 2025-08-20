namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBge(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bge       {labelName}";
}