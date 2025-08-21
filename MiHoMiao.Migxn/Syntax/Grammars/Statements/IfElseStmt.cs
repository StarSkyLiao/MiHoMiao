using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record IfElseStmt(IfToken IfToken, ParenthesizedExpr Condition, MigxnNode TrueStmt, MigxnNode FalseStmt) 
    : MigxnStmt($"if {Condition.Text} {TrueStmt.Text}\nelse {FalseStmt.Text}".AsMemory(), IfToken.Index, IfToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [IfToken, Condition, TrueStmt, FalseStmt];

    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        ReadOnlyMemory<char> labelFalse = $"<label>.if_false_{IfToken.Position}".AsMemory();
        ReadOnlyMemory<char> labelTrue = $"<label>.if_true_{IfToken.Position}".AsMemory();
        return Condition.AsOpCodes().Concat([
            new OpBrFalse(labelFalse), ..TrueStmt.AsOpCodes(), new OpGoto(labelTrue), 
            new OpLabel(labelFalse), ..FalseStmt.AsOpCodes(), new OpLabel(labelTrue),
        ]);
    }
}