namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpBne(ReadOnlyMemory<char> labelName) : MigxnOpCode
{
    public override string ToString() => $"bne       {labelName}";
}