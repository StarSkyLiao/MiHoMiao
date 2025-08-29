using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using MiHoMiao.Migxn.CodeGen;

namespace MiHoMiao.Migxn.Runtime.Members;

internal record MigxnMethod(MigxnContext Context, string Name, Type ReturnType, params Type[] MethodParams)
{
    /// <summary>
    /// 该方法体的操作码
    /// </summary>
    internal List<MigxnOpCode> Codes = [];
    
    
    
    
    /// <summary>
    /// 对应的动态方法
    /// </summary>
    [field: AllowNull, MaybeNull]
    internal DynamicMethod DynamicMethod => field ??= new DynamicMethod(Name, ReturnType, MethodParams);
    //
    // internal ILGenerator Generator => DynamicMethod.GetILGenerator();

}