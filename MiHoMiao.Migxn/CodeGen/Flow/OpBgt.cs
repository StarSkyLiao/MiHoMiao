namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBgt(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"bgt",-12}{labelName}";
}