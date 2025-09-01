namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBlt(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"blt",-12}{labelName}";
}