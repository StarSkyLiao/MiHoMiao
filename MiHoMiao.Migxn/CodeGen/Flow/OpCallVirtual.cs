using System.Reflection;

namespace MiHoMiao.Migxn.CodeGen.Flow;

internal class OpCallVirtual(MethodInfo method) : MigxnOpCode
{
    public override string ToString() => $"{"callvirt",-12}{method.Name}";
}