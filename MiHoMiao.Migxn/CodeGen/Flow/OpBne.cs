namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBne(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"bne",-12}{labelName}";
}