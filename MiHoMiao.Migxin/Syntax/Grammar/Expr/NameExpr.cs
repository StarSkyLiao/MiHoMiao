using MiHoMiao.Migxin.Syntax.Lexical;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr;

internal record NameExpr(NameToken Token) : MigxinExpr(Token.Text, Token.Index, Token.Position)
{
    internal override IEnumerable<MigxinTree> Children() => [];

    public NameToken Token { get; } = Token;

}