using System.Reflection;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpCall(MethodInfo method) : MigxnOpCode
{
    public override string ToString() => method.IsStatic ? $"{"call",-12}{"static",-9}{method}" : $"{"call",-12}{"instance",-9}{method}";
}