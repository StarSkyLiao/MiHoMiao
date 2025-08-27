using MiHoMiao.Migxin.Compiler.Lexical.Literals;

namespace MiHoMiao.Migxin.Compiler.Syntax.Expr;

internal record LiteralExpr(LiteralToken Token) : MigxinExpr(Token.Text, Token.Index, Token.Position)
{
    internal override IEnumerable<MigxinTree> Children() => [];

    public LiteralToken Token { get; } = Token;

}