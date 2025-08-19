using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record GotoToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, ILeadToken
{
    public static string UniqueName => "goto";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new GotoToken(index, position);

    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is GotoToken);
        
        SymbolToken? labelName = migxnGrammar.TryMatchToken<SymbolToken>();
        if (labelName is null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(labelName), this);
        
        return new Diagnostic<MigxnStmt>(new GotoStmt(this, labelName)); 
    }
}