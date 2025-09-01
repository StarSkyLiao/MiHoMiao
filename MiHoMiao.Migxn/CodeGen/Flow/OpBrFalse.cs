namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBrFalse(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"br.false",-12}{labelName}";
}