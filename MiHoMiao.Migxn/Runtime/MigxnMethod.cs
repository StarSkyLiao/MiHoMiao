using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Runtime;

public class MigxnMethod(string name, Type returnType, Type[] methodParams)
{
    [field: AllowNull, MaybeNull]
    internal DynamicMethod DynamicMethod => field ??= new DynamicMethod(name, returnType, methodParams);
    
    internal ILGenerator Generator => DynamicMethod.GetILGenerator();
    
    
}