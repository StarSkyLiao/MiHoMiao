using MiHoMiao.Migxn.Syntax;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Migxn.CodeAnalysis.Lexer;

/// <summary>
/// 字符串 text 为 position 开始的一个 token.
/// </summary>
public sealed class UnrecognizedToken(MigxnToken badToken) : BadMigxnTree
{
    public override string Message => $"Unrecognized token at line {badToken.Position.Line} column {badToken.Position.Column}. (:{badToken.Text}).";
    public override List<MigxnNode> MigxnTree { get; } = [badToken];

}