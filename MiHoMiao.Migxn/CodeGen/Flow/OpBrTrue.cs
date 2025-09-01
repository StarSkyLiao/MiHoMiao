namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBrTrue(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"br.true",-12}{labelName}";
}