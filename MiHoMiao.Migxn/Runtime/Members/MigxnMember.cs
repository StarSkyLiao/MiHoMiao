using MiHoMiao.Migxn.CodeGen;

namespace MiHoMiao.Migxn.Runtime.Members;

internal record MigxnMember
{
    /// <summary>
    /// 该方法体的操作码
    /// </summary>
    internal List<MigxnOpCode> Codes { get; } = [];

    /// <summary>
    /// 生成目标代码
    /// </summary>
    internal void EmitCode(MigxnOpCode code) => Codes.Add(code);
}