namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public abstract record LiteralToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)//, ILeadToken
{
    // public Result<MigxnTree> TryCollectToken(MigxnGrammar migxnGrammar)
    // {
    //     MigxnToken? begin = migxnGrammar.MoveNext();
    //     Debug.Assert(begin is LiteralToken);
    //
    //     switch (begin)
    //     {
    //         case RoundOpenToken roundOpenToken:
    //             if (!migxnGrammar.TryParseTree(out MigxnExpr? content))
    //                 return new SpecifiedTokenMissing(new BadTree([begin]), nameof(content));
    //             Debug.Assert(content != null);
    //             
    //             if(!migxnGrammar.TryMatchToken(out RoundCloseToken? roundCloseToken))
    //                 return new SpecifiedTokenMissing(new BadTree([begin, content]), ")");
    //             Debug.Assert(roundCloseToken != null);
    //             
    //             return new ParenthesizedExpr(roundOpenToken, content, roundCloseToken);
    //         case LiteralToken literalToken:
    //             TokenExpr leftNode = new TokenExpr(literalToken);
    //             MigxnToken? op = migxnGrammar.Current;
    //             switch (op)
    //             {
    //                 case IBinaryToken binaryToken:
    //                     migxnGrammar.MoveNext();
    //                     if (!migxnGrammar.TryParseTree(out MigxnExpr? rightExpr))
    //                         return new SpecifiedTokenMissing(new BadTree([leftNode, op]), nameof(rightExpr));
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