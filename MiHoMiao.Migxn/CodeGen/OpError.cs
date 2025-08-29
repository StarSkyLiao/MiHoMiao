namespace MiHoMiao.Migxn.CodeGen;

internal class OpError(string text) : MigxnOpCode
{
    public override string ToString() => $"{"ERROR",-12}{text}";
}