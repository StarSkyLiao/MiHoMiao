using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type? VisitBlockStmt(BlockStmtContext context)
    {
        MigxnContext.MigxnScope.EnterScope();
        foreach (StatementContext statementContext in context.statement()) Visit(statementContext);
        MigxnContext.MigxnScope.ExitScope();
        return null;
    }
    
}