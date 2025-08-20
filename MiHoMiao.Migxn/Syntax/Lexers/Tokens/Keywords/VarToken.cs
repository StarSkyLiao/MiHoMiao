using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record VarToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, ILeadToken
{
    public static string UniqueName => "var";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new VarToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();
    
    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is VarToken);

        SymbolToken? varName = migxnGrammar.TryMatchToken<SymbolToken>();
        // error: var
        if (varName is null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(varName), this);

        SymbolToken? varType = null;
        ColonToken? colon = migxnGrammar.TryMatchToken<ColonToken>();
        if (colon != null)
        {
            varType = migxnGrammar.TryMatchToken<SymbolToken>();
            // error: var item :
            if (varType is null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(varType),this, varName, colon);
        }
        
        // 情况 2：var item = expr
        EqualToken? equal = migxnGrammar.TryMatchToken<EqualToken>();
        if (equal == null)
        {
            // error: var item
            if (varType == null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(varName), this, varName);
            // var item : type = default
            DefaultExpr defaultExpr = new DefaultExpr(new DefaultToken(Index, Position));
            return new Diagnostic<MigxnStmt>(new VarStmt(this, varName, defaultExpr));
        }

        IResult<MigxnExpr> initialExpr = migxnGrammar.TryParse<MigxnExpr>();
        // var item : type = expr
        if (initialExpr.IsSuccess) return new Diagnostic<MigxnStmt>(new VarStmt(this, varName, initialExpr.Result!));
        // var item : type = 
        return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(initialExpr), this, varName, equal);
        
    }
}