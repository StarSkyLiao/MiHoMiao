using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Store;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record AssignStmt(MigxnExpr VarName, MigxnExpr VarValue)
    : MigxnStmt($"{VarName.Text} = {VarValue.Text}".AsMemory(), VarName.Index, VarName.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [VarName, VarValue];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => [..VarValue.AsOpCodes(context), new OpStVar(VarName.Text)];
    
}