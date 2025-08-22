using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Param;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

internal record BinaryExpr(MigxnExpr Left, IBinaryToken BinaryToken, MigxnExpr Right)
    : MigxnExpr((BinaryToken is IClosedBinaryToken
            ? $"{Left.Text}{BinaryToken.MigxnNode.Text}{Right.Text}"
            : $"({Left.Text} {BinaryToken.MigxnNode.Text} {Right.Text})"
        ).AsMemory(), Left.Index, Left.Position)
{

    public override Type ExprType(MigxnContext context) => BinaryToken.BinaryType(Left, Right, context);
    
    internal override IEnumerable<MigxnNode> Children() => [Left, BinaryToken.MigxnNode, Right];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => BinaryToken.BinaryOp(Left, Right, context);

    internal static IResult<MigxnExpr> ParseForward(MigxnExpr current, MigxnGrammar grammar)
    {
        IBinaryToken? binary = grammar.MoveNext() as IBinaryToken;
        Debug.Assert(binary is not null);
        IResult<MigxnExpr> next = grammar.TryParse<MigxnExpr>();
        if (next.IsSuccess) return new Diagnostic<MigxnExpr>(CombineBinary(current, binary, next.Result!));
        
        Debug.Assert(next.Exception != null);
        next.Exception.MigxnTree.InsertRange(0, [current, binary.MigxnNode]);
        return next;
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
            case FuncCallExpr funcCallExpr when binary.Priority == 0:
            {
                MigxnExpr binaryExpr = CombineBinary(left, binary, funcCallExpr.Method);
                return new FuncCallExpr(binaryExpr, funcCallExpr.Left, funcCallExpr.ParamList, funcCallExpr.Right);
            }
        }
        return new BinaryExpr(left, binary, right);
    }
}