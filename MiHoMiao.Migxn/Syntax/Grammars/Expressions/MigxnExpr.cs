using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public abstract record MigxnExpr(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnTree(Text, Index, Position), IExprParser<MigxnExpr>
{
    static IResult<MigxnExpr> IExprParser<MigxnExpr>.TryParse(MigxnGrammar grammar)
    {
        IResult<MigxnExpr> current = ParseCurrent(grammar);
        if (!current.IsSuccess) return current;
        
        return current;
    }

    private static IResult<MigxnExpr> ParseCurrent(MigxnGrammar grammar)
    {
        switch (grammar.Current)
        {
            case RoundOpenToken: return grammar.TryParse<ParenthesizedExpr>();
            case LiteralToken: return grammar.TryParse<TokenExpr>();
            case null: return TokenMissingException.Create<MigxnExpr>([], nameof(MigxnExpr));
        }
        return new ActionResult<MigxnExpr>(new UnexpectedTokenException<MigxnExpr>(grammar.MoveNext()!.Text.ToString()));
    }
    //
    // static bool IExprParser<MigxnExpr>.TryParse(MigxnGrammar grammar, out Result<MigxnExpr> result)
    // {
    //     MigxnToken? begin = grammar.MoveNext();
    //     Debug.Assert(begin is LiteralToken);
    //
    //     switch (begin)
    //     {
    //         case RoundOpenToken roundOpenToken:
    //             if (!grammar.TryParseTree(out MigxnExpr? content))
    //                 return new TokenMissingException(new BadTree([begin]), nameof(content));
    //             Debug.Assert(content != null);
    //             
    //             if(!grammar.TryMatchToken(out RoundCloseToken? roundCloseToken))
    //                 return new TokenMissingException(new BadTree([begin, content]), ")");
    //             Debug.Assert(roundCloseToken != null);
    //             
    //             return new ParenthesizedExpr(roundOpenToken, content, roundCloseToken);
    //         case LiteralToken literalToken:
    //             TokenExpr leftNode = new TokenExpr(literalToken);
    //             MigxnToken? op = grammar.Current;
    //             switch (op)
    //             {
    //                 case IBinaryToken binaryToken:
    //                     grammar.MoveNext();
    //                     if (!grammar.TryParseTree(out MigxnExpr? rightExpr))
    //                         return new TokenMissingException(new BadTree([leftNode, op]), nameof(rightExpr));
    //                     Debug.Assert(rightExpr != null);
    //             
    //                     if (rightExpr is not BinaryExpr rightBinaryExpr || rightBinaryExpr.BinaryToken.Priority < binaryToken.Priority) 
    //                         return new BinaryExpr(leftNode, binaryToken, rightExpr);
    //             
    //                     BinaryExpr binaryExpr = new BinaryExpr(leftNode, binaryToken, rightBinaryExpr.Left);
    //                     return new BinaryExpr(binaryExpr, rightBinaryExpr.BinaryToken, rightBinaryExpr.Right);
    //                 default:
    //                     return leftNode;
    //             }
    //     }
    //
    //     return new NotSupportedException();
    // }
}