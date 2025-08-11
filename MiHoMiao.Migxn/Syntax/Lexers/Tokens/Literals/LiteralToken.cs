using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public abstract record LiteralToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position), ILeadToken
{
    public Result<MigxnTree> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? begin = migxnGrammar.MoveNext();
        Debug.Assert(begin is LiteralToken);
        TokenExpr leftNode = new TokenExpr(begin);
        
        MigxnToken? op = migxnGrammar.Current;
        
        switch (op)
        {
            case IBinaryToken binaryToken:
                migxnGrammar.MoveNext();
                MigxnTree? next = migxnGrammar.ParseNext();
                if (next is MigxnExpr rightExpr) return new BinaryExpr(leftNode, binaryToken, rightExpr);
                return new TokenMissingException(new BadTree([leftNode, op]), nameof(rightExpr));
            default:
                return leftNode;
        }
    }
}