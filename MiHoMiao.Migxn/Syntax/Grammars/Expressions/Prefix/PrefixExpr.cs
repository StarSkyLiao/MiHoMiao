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
        
        IResult<MigxnExpr> next = grammar.TryParse<MigxnExpr>();
        if (next.IsSuccess) return new ActionResult<MigxnExpr>(CombinePrefix(prefix, next.Result!));
        List<MigxnNode> childNodes = next.Exception is IBadTreeException tree
            ? [prefix.MigxnNode, ..tree.MigxnTree.Children()]
            : [prefix.MigxnNode];
        return TokenMissingException.Create<MigxnExpr>(childNodes, nameof(MigxnExpr));
    }
    
    internal static MigxnExpr CombinePrefix(IPrefixToken prefix, MigxnExpr right)
    {
        switch (right)
        {
            case BinaryExpr nextBinary when nextBinary.BinaryToken.Priority >= prefix.Priority:
            {
                MigxnExpr binaryExpr = CombinePrefix(prefix, nextBinary.Left);
                return new BinaryExpr(binaryExpr, nextBinary.BinaryToken, nextBinary.Right);
            }
        }
        return new PrefixExpr(prefix, right);
    }
    
}