using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal interface ITokenFactory<out T> where T : MigxnToken
{
    
    public static abstract string UniqueName { get; }
    
    public static abstract T Create(int index, (int Line, int Column) position);

}

internal interface IKeywordToken : ITokenFactory<AbstractKeyword>;

internal interface IOperatorToken : ITokenFactory<MigxnOperator>;

internal interface ILeadToken
{
    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar);
    
}