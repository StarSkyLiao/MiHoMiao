using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;

internal record SuffixExpr(MigxnExpr Left, ISuffixToken SuffixToken)
    : MigxnExpr($"({Left.Text}{SuffixToken.MigxnNode.Text})".AsMemory(), Left.Index, Left.Position)
{
    public override Type ExprType(MigxnContext context) => Left.ExprType(context);

    internal override IEnumerable<MigxnNode> Children() => [Left, SuffixToken.MigxnNode];
    
    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotImplementedException();
    
    internal static IResult<MigxnExpr> ParseForward(MigxnExpr current, MigxnGrammar grammar)
    {
        ISuffixToken? suffix = grammar.MoveNext() as ISuffixToken;
        Debug.Assert(suffix is not null);
        return new Diagnostic<MigxnExpr>(new SuffixExpr(current, suffix));
    }
    
}