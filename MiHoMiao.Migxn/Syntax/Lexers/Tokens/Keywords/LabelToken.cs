using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record LabelToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, ILeadToken
{
    public static string UniqueName => "label";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new LabelToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new UnreachableException();
    
    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is LabelToken);
        
        SymbolToken? labelName = migxnGrammar.TryMatchToken<SymbolToken>();
        if (labelName is null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(labelName), this);
        
        ColonToken? colon = migxnGrammar.TryMatchToken<ColonToken>();
        if (colon is null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(colon), this);
        
        return new Diagnostic<MigxnStmt>(new LabelStmt(this, labelName)); 
    }
}