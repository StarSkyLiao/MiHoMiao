using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;

internal record PrefixExpr(IPrefixToken PrefixToken, MigxnExpr Right)
    : MigxnExpr($"({PrefixToken.MigxnNode.Text}{Right.Text})".AsMemory(), Right.Index, Right.Position)
{

    internal override IEnumerable<MigxnNode> Children() => [PrefixToken.MigxnNode, Right];
    
    internal static IResult<MigxnExpr> ParseForward(MigxnGrammar grammar)
    {
        IPrefixToken? prefix = grammar.MoveNext() as IPrefixToken;
        Debug.Assert(prefix is not null);
        
        IResult<MigxnExpr> next = ParseUnitExpr(grammar);;
        if (!next.IsSuccess)
        {
            List<MigxnNode> childNodes = [prefix.MigxnNode];
            return TokenMissingException.Create<MigxnExpr>(childNodes, nameof(MigxnExpr));
        }
        Debug.Assert(next.Result != null);
        
        if (next.Result is BinaryExpr nextBinary && nextBinary.BinaryToken.Priority > prefix.Priority)
        {
            PrefixExpr prefixExpr = new PrefixExpr(prefix, nextBinary.Left);
            return new ActionResult<MigxnExpr>(new BinaryExpr(prefixExpr, nextBinary.BinaryToken, nextBinary.Right));
        }
        
        return new ActionResult<MigxnExpr>(new PrefixExpr(prefix, next.Result));
    }
    
}