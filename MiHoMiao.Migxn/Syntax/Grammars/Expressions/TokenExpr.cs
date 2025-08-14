using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public record TokenExpr(LiteralToken Token) : MigxnExpr(Token.Text, Token.Index, Token.Position), IExprParser<TokenExpr>
{
    internal override IEnumerable<MigxnNode> Children() => [Token];

    static IResult<TokenExpr> IExprParser<TokenExpr>.TryParse(MigxnGrammar grammar)
    {
        if (grammar.MoveNext() is LiteralToken token) return new ActionResult<TokenExpr>(new TokenExpr(token));
        return new ActionResult<TokenExpr>(new TokenNotFoundException<LiteralToken>());
    }
}