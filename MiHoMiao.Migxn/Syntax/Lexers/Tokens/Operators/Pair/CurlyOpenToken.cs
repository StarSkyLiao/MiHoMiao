using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Grammars.Statements;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Pair;

public record CurlyOpenToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ILeadToken
{
    public static string UniqueName => "{";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new CurlyOpenToken(index, position);

    public IResult<MigxnStmt> TryCollectToken(MigxnGrammar migxnGrammar)
    {
        MigxnToken? token = migxnGrammar.MoveNext();
        Debug.Assert(token is CurlyOpenToken);

        List<MigxnTree> stmts = [];
        while (migxnGrammar.Current is not null and not CurlyCloseToken)
        {
            IResult<MigxnTree> unitStmt = migxnGrammar.ParseStmt();
            if (!unitStmt.IsSuccess) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(unitStmt), spanChildren:[this, ..stmts]);
            Debug.Assert(unitStmt.Result != null);
            stmts.Add(unitStmt.Result);
        }
        
        CurlyCloseToken? close = migxnGrammar.TryMatchToken<CurlyCloseToken>();
        if (close is null) return SpecifiedTokenMissing.Create<MigxnStmt>(nameof(CurlyCloseToken), spanChildren:[this, ..stmts]);

        return new Diagnostic<MigxnStmt>(new BlockStmt(this, stmts, close));
    }
}