using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;

internal record PrefixExpr(IPrefixToken PrefixToken, MigxnExpr Right)
    : MigxnExpr($"{PrefixToken.MigxnNode.Text}{Right.Text}".AsMemory(), Right.Index, Right.Position)
{

    internal override IEnumerable<MigxnNode> Children() => [PrefixToken.MigxnNode, Right];
    
    internal static IResult<MigxnExpr> ParseForward(MigxnGrammar grammar)
    {
        IPrefixToken? prefix = grammar.MoveNext() as IPrefixToken;
        Debug.Assert(prefix is not null);
        
        IResult<MigxnExpr> next = MigxnExpr.ParseUnitExpr(grammar);;
        if (!next.IsSuccess)
        {
            List<MigxnNode> childNodes = [prefix.MigxnNode];
            return TokenMissingException.Create<MigxnExpr>(childNodes, nameof(MigxnExpr));
        }

        Debug.Assert(next.Result != null);
        return new ActionResult<MigxnExpr>(new PrefixExpr(prefix, next.Result));
    }
    
}