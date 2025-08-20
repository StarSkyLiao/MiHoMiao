using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Flow;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record LoopToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, ILeadToken
{
    public static string UniqueName => "loop";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new LoopToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new UnreachableException();
    
    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is LoopToken);

        IResult<ParenthesizedExpr> times = migxnGrammar.TryParse<ParenthesizedExpr>();
        if (!times.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(times), this);
        Debug.Assert(times.Result != null);
        
        IResult<MigxnTree> stmt = migxnGrammar.ParseStmt();
        if (!stmt.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(times), this, times.Result);
        Debug.Assert(stmt.Result != null);
        
        return new Diagnostic<MigxnStmt>(new LoopStmt(this, times.Result, stmt.Result));
    }
}