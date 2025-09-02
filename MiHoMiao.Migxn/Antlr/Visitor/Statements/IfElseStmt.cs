using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type VisitIfStmt(IfStmtContext context)
    {
        Type conditionType = Visit(context.Condition);
        if (conditionType != typeof(bool))
        {
            string message = $"Can not convert {conditionType} to boolean!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Condition.Start, message));
        }
        
        string falseLabel = $"<label>.if.false.{(context.Start.Line, context.Start.Column)}";
        MigxnContext.EmitCode(new OpBrFalse(falseLabel));
        Visit(context.TrueBody);
        MigxnContext.EmitCode(new OpLabel(falseLabel));
        return typeof(void);
    }
    
    public override Type VisitIfElseStmt(IfElseStmtContext context)
    {
        Type conditionType = Visit(context.Condition);
        if (conditionType != typeof(bool))
        {
            string message = $"Can not convert {conditionType} to boolean!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Condition.Start, message));
        }
        
        string trueLabel = $"<label>.if.true.{(context.Start.Line, context.Start.Column)}";
        string falseLabel = $"<label>.if.false.{(context.Start.Line, context.Start.Column)}";
        MigxnContext.EmitCode(new OpBrFalse(falseLabel));
        Visit(context.TrueBody);
        MigxnContext.EmitCode(new OpGoto(trueLabel));
        
        MigxnContext.EmitCode(new OpLabel(falseLabel));
        Visit(context.FalseBody);
        MigxnContext.EmitCode(new OpLabel(trueLabel));
        return typeof(void);
    }
    
}