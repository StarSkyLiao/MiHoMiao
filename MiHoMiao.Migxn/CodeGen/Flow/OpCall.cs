using System.Reflection;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpCall(MethodInfo method) : MigxnOpCode
{
    public override string ToString() => $"{"call",-12}{method.Name}";
}