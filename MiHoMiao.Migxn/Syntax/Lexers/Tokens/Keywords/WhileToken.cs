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

internal record WhileToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, ILeadToken
{
    public static string UniqueName => "while";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new WhileToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();
    
    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is WhileToken);

        IResult<ParenthesizedExpr> condition = migxnGrammar.TryParse<ParenthesizedExpr>();
        if (!condition.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(condition), this);
        Debug.Assert(condition.Result != null);
        
        IResult<MigxnTree> trueStmt = migxnGrammar.ParseStmt();
        if (!trueStmt.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(condition), this, condition.Result);
        Debug.Assert(trueStmt.Result != null);
        
        return new Diagnostic<MigxnStmt>(new WhileStmt(this, condition.Result, trueStmt.Result));
    }
}