using MiHoMiao.Migxn.CodeAnalysis;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnCommonParser
{
    public override Type? VisitExprStmt(ExprStmtContext context)
    {
        if (context.expression() is AssignExprContext assignExpr) Visit(assignExpr);
        else Exceptions.Add(MigxnDiagnostic.Create(context.Start, $"Expression \"{context.GetText()}\" is not a statement."));
        return null;
    }
    
}