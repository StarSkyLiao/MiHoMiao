using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record IfToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, ILeadToken
{
    public static string UniqueName => "if";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new IfToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();
    
    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is IfToken);

        IResult<ParenthesizedExpr> condition = migxnGrammar.TryParse<ParenthesizedExpr>();
        if (!condition.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(condition), this);
        Debug.Assert(condition.Result != null);
        
        IResult<MigxnTree> trueStmt = migxnGrammar.ParseStmt();
        if (!trueStmt.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(condition), this, condition.Result);
        Debug.Assert(trueStmt.Result != null);
        
        ElseToken? elseToken = migxnGrammar.TryMatchToken<ElseToken>();
        if (elseToken is null) return new Diagnostic<MigxnStmt>(new IfStmt(this, condition.Result, trueStmt.Result));

        IResult<MigxnTree> falseStmt = migxnGrammar.ParseStmt();
        if (!falseStmt.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(condition), this, condition.Result, trueStmt.Result, elseToken);
        Debug.Assert(falseStmt.Result != null);

        return new Diagnostic<MigxnStmt>(new IfElseStmt(this, condition.Result, trueStmt.Result, falseStmt.Result));
    }
}