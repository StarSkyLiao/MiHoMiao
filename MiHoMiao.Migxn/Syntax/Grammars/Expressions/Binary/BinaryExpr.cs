using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

internal record BinaryExpr(MigxnExpr Left, IBinaryToken BinaryToken, MigxnExpr Right)
    : MigxnExpr($"({Left.Text} {BinaryToken.MigxnNode.Text} {Right.Text})".AsMemory(), Left.Index, Left.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Left, BinaryToken.MigxnNode, Right];
    
    internal static IResult<MigxnExpr> ParseForward(MigxnExpr current, MigxnGrammar grammar)
    {
        IBinaryToken? binary = grammar.MoveNext() as IBinaryToken;
        Debug.Assert(binary is not null);
        IResult<MigxnExpr> next = ParseUnitExpr(grammar);
        if (!next.IsSuccess)
        {
            List<MigxnNode> childNodes = [current, binary.MigxnNode];
            return TokenMissingException.Create<MigxnExpr>(childNodes, nameof(MigxnExpr));
        }

        switch (next.Result)
        {
            case BinaryExpr nextBinary when nextBinary.BinaryToken.Priority >= binary.Priority:
            {
                BinaryExpr binaryExpr = new BinaryExpr(current, binary, nextBinary.Left);
                return new ActionResult<MigxnExpr>(new BinaryExpr(binaryExpr, nextBinary.BinaryToken, nextBinary.Right));
            }
            case SuffixExpr nextSuffix when nextSuffix.SuffixToken.Priority >= binary.Priority:
            {
                BinaryExpr binaryExpr = new BinaryExpr(current, binary, nextSuffix.Left);
                return new ActionResult<MigxnExpr>(new SuffixExpr(binaryExpr, nextSuffix.SuffixToken));
            }
            default:
                return new ActionResult<MigxnExpr>(new BinaryExpr(current, binary, next.Result!));
        }
    }
    
}