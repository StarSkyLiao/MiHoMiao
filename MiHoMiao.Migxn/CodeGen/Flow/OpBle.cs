namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBle(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"ble",-12}{labelName}";
}