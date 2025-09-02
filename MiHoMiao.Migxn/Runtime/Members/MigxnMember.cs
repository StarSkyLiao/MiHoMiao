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
    internal void EmitCode(MigxnOpCode code) => code.OnEmitting(Codes, Codes.Count);

    /// <summary>
    /// 生成目标代码
    /// </summary>
    internal void InsertEmitCode(int index, MigxnOpCode code) => code.OnEmitting(Codes, index);
}