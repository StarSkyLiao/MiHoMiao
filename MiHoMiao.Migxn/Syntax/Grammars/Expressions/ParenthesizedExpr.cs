using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public record ParenthesizedExpr(RoundOpenToken Left, MigxnExpr Content, RoundCloseToken Right) 
    : MigxnExpr($"({Content.Text})".AsMemory(), Left.Index, Left.Position), IExprParser<ParenthesizedExpr>
{
    internal override IEnumerable<MigxnNode?> Children() => [Left, Content, Right];

    static IResult<ParenthesizedExpr> IExprParser<ParenthesizedExpr>.TryParse(MigxnGrammar grammar)
    {
        RoundOpenToken? openToken = grammar.MoveNext() as RoundOpenToken;
        Debug.Assert(openToken != null);

        IResult<MigxnExpr> content = grammar.TryParse<MigxnExpr>();
        if (!content.IsSuccess) return new ActionResult<ParenthesizedExpr>(content.Exception!);
        Debug.Assert(content.Result != null);

        RoundCloseToken? closeToken = grammar.TryMatchToken<RoundCloseToken>();
        if (closeToken is null) return TokenMissingException.Create<ParenthesizedExpr>([openToken, content.Result], ")");
       
        return new ActionResult<ParenthesizedExpr>(new ParenthesizedExpr(openToken, content.Result, closeToken));
    }
}