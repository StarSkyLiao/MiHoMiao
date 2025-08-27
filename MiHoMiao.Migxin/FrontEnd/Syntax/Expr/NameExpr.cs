using MiHoMiao.Migxin.FrontEnd.Lexical.Names;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Expr;

internal record NameExpr(NameToken Token) : MigxinExpr(Token.Text, Token.Index, Token.Position)
{
    internal override IEnumerable<MigxinTree> Children() => [];

    public NameToken Token { get; } = Token;

}