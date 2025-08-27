using MiHoMiao.Migxin.Compiler.Syntax.Expr;
using MiHoMiao.Migxin.Compiler.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.Compiler.Syntax.Stmt;

internal record AssignStmt(MigxinExpr Var, AssignSymbol AssignSymbol, MigxinExpr Expr)
    : MigxinStmt($"{Var.Text}{AssignSymbol.Text}{Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Expr];
    
}