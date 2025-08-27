using MiHoMiao.Migxin.FrontEnd.Syntax.Expr;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Stmt;

internal record AssignStmt(MigxinExpr Var, AssignSymbol AssignSymbol, MigxinExpr Expr)
    : MigxinStmt($"{Var.Text}{AssignSymbol.Text}{Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Expr];
    
}