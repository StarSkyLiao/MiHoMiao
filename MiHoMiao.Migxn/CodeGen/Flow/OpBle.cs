namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBle(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"{"ble",-12}{labelName}";
}