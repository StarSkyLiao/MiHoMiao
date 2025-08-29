namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBrTrue(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"br.true   {labelName}";
}