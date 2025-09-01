using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type? VisitLoopStmt(LoopStmtContext context)
    {
        string countVar = $"<var>.loop.count.{(context.Start.Line, context.Start.Column)}";
        string lengthVar = $"<var>.loop.length.{(context.Start.Line, context.Start.Column)}";
        string endLabel = $"<label>.loop.end.{(context.Start.Line, context.Start.Column)}";
        string[] caseLabel = new string[8];
        for (int i = 0; i < 8; i++) caseLabel[i] =  $"<label>.loop.case.{i}.{(context.Start.Line, context.Start.Column)}";

        Visit(context.LoopTimes);
        MigxnContext.EmitCode(new OpDup());
        MigxnContext.EmitCode(new OpStVar(lengthVar));
        MigxnContext.EmitCode(new OpLdcLong(0));
        
        MigxnContext.EmitCode(new OpBlt(endLabel));
        MigxnContext.EmitCode(new OpLdVar(lengthVar));
        MigxnContext.EmitCode(new OpLdcLong(7));
        MigxnContext.EmitCode(new OpAdd());
        MigxnContext.EmitCode(new OpLdcLong(8));
        MigxnContext.EmitCode(new OpDiv());
        MigxnContext.EmitCode(new OpStVar(countVar));
        
        
        MigxnContext.EmitCode(new OpSwitch(caseLabel));
        
        MigxnContext.EmitCode(new OpLabel(caseLabel[0]));
        Visit(context.LoopBody);
        for (int i = 7; i >= 1; --i)
        {
            MigxnContext.EmitCode(new OpLabel(caseLabel[i]));
            Visit(context.LoopBody);
        }

        MigxnContext.EmitCode(new OpLdVar(countVar));
        MigxnContext.EmitCode(new OpLdcLong(-1));
        MigxnContext.EmitCode(new OpSub());
        MigxnContext.EmitCode(new OpDup());
        MigxnContext.EmitCode(new OpStVar(countVar));
        MigxnContext.EmitCode(new OpLdcLong(0));
        MigxnContext.EmitCode(new OpBgt(caseLabel[0]));
        MigxnContext.EmitCode(new OpLabel(endLabel));
        
        return null;
    }
    
}