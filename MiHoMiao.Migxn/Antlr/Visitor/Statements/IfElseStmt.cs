using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnCommonParser
{
    public override Type? VisitIfStmt(IfStmtContext context)
    {
        Visit(context.Condition);
        string falseLabel = $"<label>.if.false.{(context.Start.Line, context.Start.Column)}";
        Codes.Add(new OpBrFalse(falseLabel));
        Visit(context.TrueBody);
        Codes.Add(new OpLabel(falseLabel));
        return null;
    }
    
    public override Type? VisitIfElseStmt(IfElseStmtContext context)
    {
        Visit(context.Condition);
        string trueLabel = $"<label>.if.true.{(context.Start.Line, context.Start.Column)}";
        string falseLabel = $"<label>.if.false.{(context.Start.Line, context.Start.Column)}";
        Codes.Add(new OpBrFalse(falseLabel));
        Visit(context.TrueBody);
        Codes.Add(new OpGoto(trueLabel));
        
        Codes.Add(new OpLabel(falseLabel));
        Visit(context.FalseBody);
        Codes.Add(new OpLabel(trueLabel));
        return null;
    }
    
}