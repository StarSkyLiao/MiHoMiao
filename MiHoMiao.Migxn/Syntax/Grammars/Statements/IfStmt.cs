using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record IfStmt(IfToken IfToken, ParenthesizedExpr Condition, MigxnNode TrueStmt) 
    : MigxnStmt($"if {Condition.Text} {TrueStmt.Text}".AsMemory(), IfToken.Index, IfToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [IfToken, Condition, TrueStmt];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        ReadOnlyMemory<char> labelFalse = $"<label>.if_false_{IfToken.Position}".AsMemory();
        return Condition.AsOpCodes(context).Concat([
            new OpBrFalse(labelFalse), ..TrueStmt.AsOpCodes(context), new OpLabel(labelFalse)
        ]);
    }
}