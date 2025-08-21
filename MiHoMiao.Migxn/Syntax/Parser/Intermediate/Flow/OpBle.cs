namespace MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

internal class OpBle(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"ble       {labelName}";
}