namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpGoto(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.goto   {labelName}";
}