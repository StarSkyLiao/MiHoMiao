using MiHoMiao.Migxin.Compiler.Lexical.Names;

namespace MiHoMiao.Migxin.Compiler.Semantic.Variables;

internal record MigxinVariable(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position, Type VarType)
    : NameToken(Text, Index, Position)
{
    /// <summary>
    /// 该变量是否可写
    /// </summary>
    public bool IsWritable { get; set; }

    /// <summary>
    /// 从 nameToken 和 varType 创造变量
    /// </summary>
    public static MigxinVariable Create(NameToken nameToken, Type varType) => new MigxinVariable(
        nameToken.Text, nameToken.Index, nameToken.Position, varType
    );

}