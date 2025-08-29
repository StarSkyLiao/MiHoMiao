using static MiHoMiao.Migxn.Antlr.Generated.MigxnStmt;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnLanguage
{
    public override Type? VisitBlockStmt(BlockStmtContext context)
    {
        Scopes.EnterScope();
        foreach (StatementContext statementContext in context.statement()) Visit(statementContext);
        Scopes.ExitScope();
        return null;
    }
    
}