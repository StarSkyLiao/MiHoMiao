using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

internal record TokenExpr(LiteralToken Token) : MigxnExpr(Token.Text, Token.Index, Token.Position), IExprParser<TokenExpr>
{
    public override Type ExprType(MigxnContext context) => Token.LiteralType(context);

    internal override IEnumerable<MigxnNode> Children() => [Token];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => Token.AsOpCodes();

    static IResult<TokenExpr> IExprParser<TokenExpr>.TryParse(MigxnGrammar grammar)
    {
        MigxnToken? migxnToken = grammar.MoveNext();
        if (migxnToken is LiteralToken token) return new Diagnostic<TokenExpr>(new TokenExpr(token));
        return SpecifiedTokenMissing.Create<TokenExpr>(nameof(LiteralToken));
    }
}