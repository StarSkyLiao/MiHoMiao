using MiHoMiao.Migxin.Syntax.Lexical.Literals;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr;

internal record LiteralExpr(LiteralToken Token) : MigxinExpr(Token.Text, Token.Index, Token.Position)
{
    internal override IEnumerable<MigxinTree> Children() => [];

    public LiteralToken Token { get; } = Token;

}