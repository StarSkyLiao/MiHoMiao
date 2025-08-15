using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
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
        if (next.IsSuccess) return new Diagnostic<MigxnExpr>(CombinePrefix(prefix, next.Result!));
        
        Debug.Assert(next.Exception != null);
        next.Exception.MigxnTree.Insert(0, prefix.MigxnNode);
        return next;
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