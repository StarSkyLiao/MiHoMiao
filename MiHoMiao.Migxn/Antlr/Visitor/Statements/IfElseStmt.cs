using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type? VisitIfStmt(IfStmtContext context)
    {
        Visit(context.Condition);
        string falseLabel = $"<label>.if.false.{(context.Start.Line, context.Start.Column)}";
        MigxnContext.EmitCode(new OpBrFalse(falseLabel));
        Visit(context.TrueBody);
        MigxnContext.EmitCode(new OpLabel(falseLabel));
        return null;
    }
    
    public override Type? VisitIfElseStmt(IfElseStmtContext context)
    {
        Visit(context.Condition);
        string trueLabel = $"<label>.if.true.{(context.Start.Line, context.Start.Column)}";
        string falseLabel = $"<label>.if.false.{(context.Start.Line, context.Start.Column)}";
        MigxnContext.EmitCode(new OpBrFalse(falseLabel));
        Visit(context.TrueBody);
        MigxnContext.EmitCode(new OpGoto(trueLabel));
        
        MigxnContext.EmitCode(new OpLabel(falseLabel));
        Visit(context.FalseBody);
        MigxnContext.EmitCode(new OpLabel(trueLabel));
        return null;
    }
    
}