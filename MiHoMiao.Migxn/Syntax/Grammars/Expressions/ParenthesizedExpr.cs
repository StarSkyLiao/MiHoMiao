using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Pair;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

internal record ParenthesizedExpr(RoundOpenToken Left, MigxnExpr Content, RoundCloseToken Right) 
    : MigxnExpr($"({Content.Text})".AsMemory(), Left.Index, Left.Position), IExprParser<ParenthesizedExpr>
{
    public override Type ExprType(MigxnContext context) => Content.ExprType(context);

    internal override IEnumerable<MigxnNode> Children() => [Left, Content, Right];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => Content.AsOpCodes();

    static IResult<ParenthesizedExpr> IExprParser<ParenthesizedExpr>.TryParse(MigxnGrammar grammar)
    {
        RoundOpenToken? openToken = grammar.MoveNext() as RoundOpenToken;
        Debug.Assert(openToken != null);

        IResult<MigxnExpr> content = grammar.TryParse<MigxnExpr>();
        if (!content.IsSuccess) return new Diagnostic<ParenthesizedExpr>(content.Exception!);
        Debug.Assert(content.Result != null);

        RoundCloseToken? closeToken = grammar.TryMatchToken<RoundCloseToken>();
        if (closeToken is null) return SpecifiedTokenMissing.Create<ParenthesizedExpr>(")", openToken, content.Result);
       
        return new Diagnostic<ParenthesizedExpr>(new ParenthesizedExpr(openToken, content.Result, closeToken));
    }
}