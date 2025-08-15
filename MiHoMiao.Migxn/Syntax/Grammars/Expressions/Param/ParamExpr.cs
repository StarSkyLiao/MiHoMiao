using System.Diagnostics;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Param;

public record ParamExpr(MigxnExpr Method, RoundOpenToken Left, List<MigxnNode> ParamList, RoundCloseToken Right)
    : MigxnExpr($"{Method.Text}{ParamList.GenericViewer(
        item => item.Text.ToString(), "(", ")", ", "
    )}".AsMemory(), Left.Index, Left.Position)
{

    internal override IEnumerable<MigxnNode> Children() => [Method, Left, ..ParamList, Right];
    
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
                    if (node is null && paramNodes.Count > 0) return TokenMissingException.Create<MigxnExpr>(paramNodes, nameof(MigxnExpr));
                    if (node is null) return new ActionResult<MigxnExpr>(new ParamExpr(method, left, paramList, right));
                    paramList.Add(node);
                    return new ActionResult<MigxnExpr>(new ParamExpr(method, left, paramList, right));
                case CommaToken:
                    paramNodes.Add(grammar.MoveNext()!);
                    if (node is null) return TokenMissingException.Create<MigxnExpr>(paramNodes, nameof(MigxnExpr));
                    paramList.Add(node);
                    node = null;
                    break;
                default:
                    IResult<MigxnExpr> result = grammar.TryParse<MigxnExpr>();
                    if (!result.IsSuccess) return TokenMissingException.Create<MigxnExpr>(paramNodes, nameof(MigxnExpr));
                    node = result.Result!;
                    paramNodes.Add(node);
                    break;
            }
        }
        return TokenMissingException.Create<MigxnExpr>(paramNodes, nameof(RoundCloseToken));
    }
    
}