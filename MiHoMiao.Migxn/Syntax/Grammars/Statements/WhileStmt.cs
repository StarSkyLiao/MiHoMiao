using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Flow;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record WhileStmt(WhileToken WhileToken, ParenthesizedExpr Condition, MigxnNode TrueStmt) 
    : MigxnStmt($"while {Condition.Text} {TrueStmt.Text}".AsMemory(), WhileToken.Index, WhileToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [WhileToken, Condition, TrueStmt];

    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        ReadOnlyMemory<char> labelStart = $"<label>.while_start_{WhileToken.Position}".AsMemory();
        ReadOnlyMemory<char> labelCondition = $"<label>.while_condition_{WhileToken.Position}".AsMemory();
        return [
            new OpGoto(labelCondition), new OpLabel(labelStart),
            ..TrueStmt.AsOpCodes(), new OpLabel(labelCondition), ..Condition.AsOpCodes(),
            new OpBrTrue(labelStart),
        ];
    }
}