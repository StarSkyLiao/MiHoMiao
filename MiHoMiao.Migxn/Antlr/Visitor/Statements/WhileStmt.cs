using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type VisitWhileStmt(WhileStmtContext context)
    {
        string startLabel = $"<label>.while.start.{(context.Start.Line, context.Start.Column)}";
        string conditionLabel = $"<label>.while.condition.{(context.Start.Line, context.Start.Column)}";
        
        MigxnContext.EmitCode(new OpGoto(conditionLabel));
        MigxnContext.EmitCode(new OpLabel(startLabel));
        
        Visit(context.WhileBody);

        MigxnContext.EmitCode(new OpLabel(conditionLabel));
        Type conditionType = Visit(context.Condition);
        if (conditionType != typeof(bool))
        {
            string message = $"Can not convert {conditionType} to boolean!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Condition.Start, message));
        }
        
        MigxnContext.EmitCode(new OpBrTrue(startLabel));
        
        return typeof(void);
    }
    
}