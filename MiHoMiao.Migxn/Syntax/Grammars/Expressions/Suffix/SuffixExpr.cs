using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;

internal record SuffixExpr(MigxnExpr Left, ISuffixToken SuffixToken)
    : MigxnExpr($"({Left.Text}{SuffixToken.MigxnNode.Text})".AsMemory(), Left.Index, Left.Position)
{

    internal override IEnumerable<MigxnNode> Children() => [Left, SuffixToken.MigxnNode];
    
    internal static IResult<MigxnExpr> ParseForward(MigxnExpr current, MigxnGrammar grammar)
    {
        ISuffixToken? suffix = grammar.MoveNext() as ISuffixToken;
        Debug.Assert(suffix is not null);
        return new ActionResult<MigxnExpr>(new SuffixExpr(current, suffix));
    }
    
}