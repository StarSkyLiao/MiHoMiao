namespace MiHoMiao.Migxn.Syntax.Intermediate.Flow;

public class OpBle(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"ble       {labelName}";
}