using MiHoMiao.Migxin.Syntax.Grammar.Expr;
using MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;
using MiHoMiao.Migxin.Syntax.Lexical.Keywords.Core;
using MiHoMiao.Migxin.Syntax.Lexical.Names;

namespace MiHoMiao.Migxin.Syntax.Grammar.Stmt;

internal record AssignStmt(MigxinExpr Var, AssignSymbol AssignSymbol, MigxinExpr Expr)
    : MigxinStmt($"{Var.Text}{AssignSymbol.Text}{Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Expr];
    
}