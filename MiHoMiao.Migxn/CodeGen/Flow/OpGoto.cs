namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpGoto(string labelName) : MigxnOpCode
{
    public override string ToString() => $"{"br.goto",-12}{labelName}";
}