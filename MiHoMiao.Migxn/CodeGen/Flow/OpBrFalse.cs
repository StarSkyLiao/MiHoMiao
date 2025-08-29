namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBrFalse(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.false  {labelName}";
}