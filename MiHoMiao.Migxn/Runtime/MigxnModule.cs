using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Runtime;

/// <summary>
/// 代表了 Migxn 的一个模块.
/// </summary>
public class MigxnModule
{
    private readonly Dictionary<string, MethodInfo> m_Methods = [];
    
    /// <summary>
    /// 代表了 Migxn 的一个模块.
    /// </summary>
    public MigxnModule(string name, ModuleBuilder moduleBuilder)
    {
        ModuleName = name;
        TypeBuilder = moduleBuilder.DefineType(ModuleName, TypeAttributes.Public | TypeAttributes.Class);
    }

    /// <summary>
    /// 模块的名字
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// 该模块的类型构建器
    /// </summary>
    [field: AllowNull, MaybeNull]
    internal TypeBuilder TypeBuilder { get; }
    
    /// <summary>
    /// 该模块的类型信息
    /// </summary>
    public TypeInfo? TypeInfo { get; private set; }

    /// <summary>
    /// 编译并固定类型
    /// </summary>
    public bool Build()
    {
        if (TypeInfo is not null) return false;
        TypeInfo = TypeBuilder.CreateTypeInfo();
        return true;
    }
    
    /// <summary>
    /// 查询指定的方法
    /// </summary>
    public MethodInfo? GetMethod(string methodName) => m_Methods.GetValueOrDefault(methodName);
    
    // /// <summary>
    // /// 编译指定的方法体
    // /// </summary>
    // public MethodInfo CompileMethod(string name, MigxnTree migxnTree)
    // {
    //     MethodBuilder methodBuilder = TypeBuilder.DefineMethod(name,
    //         MethodAttributes.Public | MethodAttributes.Static
    //     );
    //     MigxnContext context = new MigxnContext();
    //     
    //     ILGenerator ilGenerator = methodBuilder.GetILGenerator();
    //     foreach (MigxnNode node in migxnTree.Nodes) node.EmitCode(context, ilGenerator);
    //     ilGenerator.Emit(OpCodes.Ret);
    //     return m_Methods[name] = methodBuilder;
    // }
    
}