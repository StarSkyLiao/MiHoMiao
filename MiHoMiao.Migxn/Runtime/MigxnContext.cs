using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Runtime;

internal class MigxnContext
{
    /// <summary>
    /// 该上下文环境的的作用域
    /// </summary>
    internal readonly MigxnScope MigxnScope = new MigxnScope();

    /// <summary>
    /// 当前的成员
    /// </summary>
    internal List<MigxnMember> AllMembers { get; set; } = [];

    /// <summary>
    /// 当前的成员
    /// </summary>
    internal MigxnMember MigxnMember => AllMembers[^1];

    /// <summary>
    /// 抛出的解析错误
    /// </summary>
    internal readonly List<MigxnDiagnostic> Exceptions = [];
    
    /// <summary>
    /// 发现的解析警告
    /// </summary>
    internal readonly List<MigxnDiagnostic> Warnings = [];
    
    /// <summary>
    /// 生成目标代码
    /// </summary>
    internal void EmitCode(MigxnOpCode code) => MigxnMember.EmitCode(code);
    
    /// <summary>
    /// 向一个标记的指令位置插入代码
    /// </summary>
    internal void InsertEmitCode(int index, MigxnOpCode code) => MigxnMember.InsertEmitCode(index, code);
    
}