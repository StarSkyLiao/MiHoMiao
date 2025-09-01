using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnCommonParser
{
    public override Type? VisitLoopStmt(LoopStmtContext context)
    {
        string countVar = $"<var>.loop.count.{(context.Start.Line, context.Start.Column)}";
        string lengthVar = $"<var>.loop.length.{(context.Start.Line, context.Start.Column)}";
        string endLabel = $"<label>.loop.end.{(context.Start.Line, context.Start.Column)}";
        string[] caseLabel = new string[8];
        for (int i = 0; i < 8; i++) caseLabel[i] =  $"<label>.loop.case.{i}.{(context.Start.Line, context.Start.Column)}";

        Visit(context.LoopTimes);
        Codes.Add(new OpDup());
        Codes.Add(new OpStVar(lengthVar));
        Codes.Add(new OpLdcLong(0));
        
        Codes.Add(new OpBlt(endLabel));
        Codes.Add(new OpLdVar(lengthVar));
        Codes.Add(new OpLdcLong(7));
        Codes.Add(new OpAdd());
        Codes.Add(new OpLdcLong(8));
        Codes.Add(new OpDiv());
        Codes.Add(new OpStVar(countVar));
        
        
        Codes.Add(new OpSwitch(caseLabel));
        
        Codes.Add(new OpLabel(caseLabel[0]));
        Visit(context.LoopBody);
        for (int i = 7; i >= 1; --i)
        {
            Codes.Add(new OpLabel(caseLabel[i]));
            Visit(context.LoopBody);
        }

        Codes.Add(new OpLdVar(countVar));
        Codes.Add(new OpLdcLong(-1));
        Codes.Add(new OpSub());
        Codes.Add(new OpDup());
        Codes.Add(new OpStVar(countVar));
        Codes.Add(new OpLdcLong(0));
        Codes.Add(new OpBgt(caseLabel[0]));
        Codes.Add(new OpLabel(endLabel));
        
        return null;
    }
    
}