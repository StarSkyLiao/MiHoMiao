namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpGoto(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"goto      {labelName}";
}