using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnCommonParser
{
    public override Type? VisitWhileStmt(WhileStmtContext context)
    {
        string startLabel = $"<label>.while.start.{(context.Start.Line, context.Start.Column)}";
        string conditionLabel = $"<label>.while.condition.{(context.Start.Line, context.Start.Column)}";
        
        Codes.Add(new OpGoto(conditionLabel));
        Codes.Add(new OpLabel(startLabel));
        
        Visit(context.WhileBody);

        Codes.Add(new OpLabel(conditionLabel));
        Visit(context.Condition);
        
        Codes.Add(new OpBrTrue(startLabel));
        
        return null;
    }
    
}