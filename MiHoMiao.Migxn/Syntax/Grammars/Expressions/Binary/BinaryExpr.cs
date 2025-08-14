using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

internal record BinaryExpr(MigxnExpr Left, IBinaryToken BinaryToken, MigxnExpr Right)
    : MigxnExpr($"{Left.Text} {BinaryToken.MigxnNode.Text} {Right.Text}".AsMemory(), Left.Index, Left.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Left, BinaryToken.MigxnNode, Right];
    
    internal static IResult<MigxnExpr> ParseForward(MigxnExpr current, MigxnGrammar grammar)
    {
        IBinaryToken? binary = grammar.MoveNext() as IBinaryToken;
        Debug.Assert(binary is not null);
        IResult<MigxnExpr> next = grammar.TryParse<MigxnExpr>();
        if (!next.IsSuccess)
        {
            List<MigxnNode> childNodes = [current, binary.MigxnNode];
            return TokenMissingException.Create<MigxnExpr>(childNodes, nameof(MigxnExpr));
        }

        if (next.Result is BinaryExpr nextBinary)
        {
            if (nextBinary.BinaryToken.Priority < binary.Priority)
                return new ActionResult<MigxnExpr>(new BinaryExpr(current, binary, next.Result!));

            BinaryExpr binaryExpr = new BinaryExpr(current, binary, nextBinary.Left);
            return new ActionResult<MigxnExpr>(new BinaryExpr(binaryExpr, nextBinary.BinaryToken, nextBinary.Right));
        }
        // if (next.Result is SuffixExpr nextSuffix)
        // {
        //     if (nextSuffix.BinaryToken.Priority < binary.Priority)
        //         return new ActionResult<MigxnExpr>(new BinaryExpr(current, binary, next.Result!));
        //
        //     BinaryExpr binaryExpr = new BinaryExpr(current, binary, nextBinary.Left);
        //     return new ActionResult<MigxnExpr>(new BinaryExpr(binaryExpr, nextBinary.BinaryToken, nextBinary.Right));
        // }

        return new ActionResult<MigxnExpr>(new BinaryExpr(current, binary, next.Result!));
    }
    
}