namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpLabel(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"label     {labelName}";
}