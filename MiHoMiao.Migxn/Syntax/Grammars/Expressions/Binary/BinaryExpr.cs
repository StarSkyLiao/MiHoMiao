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
        IResult<MigxnExpr> next = grammar.TryParse<MigxnExpr>();
        if (next.IsSuccess) return new ActionResult<MigxnExpr>(CombineBinary(current, binary, next.Result!));
        List<MigxnNode> childNodes = next.Exception is IBadTreeException tree
            ? [current, binary.MigxnNode, ..tree.MigxnTree.Children()]
            : [current, binary.MigxnNode];
        return TokenMissingException.Create<MigxnExpr>(childNodes, nameof(MigxnExpr));
    }
    
    internal static MigxnExpr CombineBinary(MigxnExpr left, IBinaryToken binary, MigxnExpr right)
    {
        switch (right)
        {
            case BinaryExpr nextBinary when nextBinary.BinaryToken.Priority >= binary.Priority:
            {
                MigxnExpr binaryExpr = CombineBinary(left, binary, nextBinary.Left);
                return new BinaryExpr(binaryExpr, nextBinary.BinaryToken, nextBinary.Right);
            }
            case SuffixExpr nextSuffix when nextSuffix.SuffixToken.Priority >= binary.Priority:
            {
                BinaryExpr binaryExpr = new BinaryExpr(left, binary, nextSuffix.Left);
                return new SuffixExpr(binaryExpr, nextSuffix.SuffixToken);
            }
        }
        return new BinaryExpr(left, binary, right);
    }
    
}