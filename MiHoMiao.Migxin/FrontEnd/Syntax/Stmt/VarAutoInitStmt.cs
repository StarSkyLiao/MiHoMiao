using MiHoMiao.Migxin.FrontEnd.Lexical.Keywords.Core;
using MiHoMiao.Migxin.FrontEnd.Lexical.Names;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Stmt;

internal record VarAutoInitStmt(VarToken Var, NameToken Identifier, MigxinExpr Expr)
    : MigxinStmt($"var {Identifier.Text} = {Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Identifier, Expr];
    
}