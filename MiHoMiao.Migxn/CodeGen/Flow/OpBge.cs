namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBge(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"bge",-12}{labelName}";
}