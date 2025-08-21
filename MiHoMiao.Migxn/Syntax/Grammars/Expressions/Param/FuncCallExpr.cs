using System.Diagnostics;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Pair;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Param;

internal record FuncCallExpr(MigxnExpr Method, RoundOpenToken Left, List<MigxnNode> ParamList, RoundCloseToken Right)
    : MigxnExpr($"{Method.Text}{ParamList.GenericViewer(
        item => item.Text.ToString(), "(", ")", ", "
    )}".AsMemory(), Left.Index, Left.Position)
{
    // todo
    public override Type ExprType(MigxnContext context) => typeof(object);

    internal override IEnumerable<MigxnNode> Children() => [Method, Left, ..ParamList, Right];
    
    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => throw new NotImplementedException();
    
    internal static IResult<MigxnExpr> ParseForward(MigxnExpr method, MigxnGrammar grammar)
    {
        RoundOpenToken? left = grammar.MoveNext() as RoundOpenToken;
        Debug.Assert(left is not null);
        List<MigxnNode> paramNodes = [method, left];
        List<MigxnNode> paramList  = [];

        MigxnNode? node = null;
        while (grammar.Current is not null)
        {
            switch (grammar.Current)
            {
                case RoundCloseToken right:
                    paramNodes.Add(grammar.MoveNext()!);
                    if (node is null && paramNodes.Count > 0) return SpecifiedTokenMissing.Create<MigxnExpr>(nameof(MigxnExpr), paramNodes);
                    if (node is null) return new Diagnostic<MigxnExpr>(new FuncCallExpr(method, left, paramList, right));
                    paramList.Add(node);
                    return new Diagnostic<MigxnExpr>(new FuncCallExpr(method, left, paramList, right));
                case CommaToken:
                    paramNodes.Add(grammar.MoveNext()!);
                    if (node is null) return SpecifiedTokenMissing.Create<MigxnExpr>(nameof(MigxnExpr), paramNodes);
                    paramList.Add(node);
                    node = null;
                    break;
                default:
                    IResult<MigxnExpr> result = grammar.TryParse<MigxnExpr>();
                    if (!result.IsSuccess) return SpecifiedTokenMissing.Create<MigxnExpr>(nameof(MigxnExpr), paramNodes);
                    node = result.Result!;
                    paramNodes.Add(node);
                    break;
            }
        }
        if (node is null && paramNodes.Count > 0) return SpecifiedTokenMissing.Create<MigxnExpr>(nameof(MigxnExpr), paramNodes);
        return SpecifiedTokenMissing.Create<MigxnExpr>(nameof(RoundCloseToken), paramNodes);
    }
    
}