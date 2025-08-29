using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Runtime;

internal class MigxnContext
{
    /// <summary>
    /// 该上下文环境的的作用域
    /// </summary>
    internal readonly MigxnScope MigxnScope = new MigxnScope();
    
    /// <summary>
    /// 抛出的解析错误
    /// </summary>
    internal readonly List<MigxnDiagnostic> Exceptions = [];
    
    /// <summary>
    /// 发现的解析警告
    /// </summary>
    internal readonly List<MigxnDiagnostic> Warnings = [];
    
}