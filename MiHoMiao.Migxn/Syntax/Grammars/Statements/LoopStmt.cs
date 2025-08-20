using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Store;
using MiHoMiao.Migxn.Syntax.Intermediate.Flow;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record LoopStmt(LoopToken LoopToken, ParenthesizedExpr LoopTimes, MigxnNode ContainedStmt) 
    : MigxnStmt($"loop {LoopTimes.Text} {ContainedStmt.Text}".AsMemory(), LoopToken.Index, LoopToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [LoopToken, LoopTimes, ContainedStmt];

    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        ReadOnlyMemory<char> varLoopN = $"<var>.loop_n_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> varLength = $"<var>.loop_length_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> labelEnd = $"<label>.loop_end_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label0 = $"<label>.loop_case_0_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label1 = $"<label>.loop_case_1_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label2 = $"<label>.loop_case_2_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label3 = $"<label>.loop_case_3_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label4 = $"<label>.loop_case_4_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label5 = $"<label>.loop_case_5_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label6 = $"<label>.loop_case_6_{LoopToken.Position}".AsMemory();
        ReadOnlyMemory<char> label7 = $"<label>.loop_case_7_{LoopToken.Position}".AsMemory();
        
        return [
            ..LoopTimes.AsOpCodes(), new OpDup(), new OpStVar(varLength), new OpLdcI4S(0),
            new OpBlt(labelEnd),
            new OpLdVar(varLength), new OpLdcI4S(7), new OpAdd(), new OpLdcI4S(8), new OpDiv(), new OpStVar(varLoopN),
            new OpSwitch([label0, label1, label2, label3, label4, label5, label6, label7]),
            new OpLabel(label0), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label7), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label6), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label5), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label4), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label3), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label2), ..ContainedStmt.AsOpCodes(),
            new OpLabel(label1), ..ContainedStmt.AsOpCodes(),
            new OpLdVar(varLoopN), new OpLdcI4S(-1), new OpSub(), new OpDup(), new OpStVar(varLoopN), new OpLdcI4S(0),
            new OpBgt(label0),
            new OpLabel(labelEnd),
        ];
    }
}