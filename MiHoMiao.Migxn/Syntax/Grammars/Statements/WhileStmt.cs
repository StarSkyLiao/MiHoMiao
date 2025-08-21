using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record WhileStmt(WhileToken WhileToken, ParenthesizedExpr Condition, MigxnNode TrueStmt) 
    : MigxnStmt($"while {Condition.Text} {TrueStmt.Text}".AsMemory(), WhileToken.Index, WhileToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [WhileToken, Condition, TrueStmt];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        ReadOnlyMemory<char> labelStart = $"<label>.while_start_{WhileToken.Position}".AsMemory();
        ReadOnlyMemory<char> labelCondition = $"<label>.while_condition_{WhileToken.Position}".AsMemory();
        return [
            new OpGoto(labelCondition), new OpLabel(labelStart),
            ..TrueStmt.AsOpCodes(context), new OpLabel(labelCondition), ..Condition.AsOpCodes(context),
            new OpBrTrue(labelStart),
        ];
    }
}