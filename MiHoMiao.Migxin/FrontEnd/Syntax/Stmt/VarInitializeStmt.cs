using MiHoMiao.Migxin.FrontEnd.Lexical.Keywords.Core;
using MiHoMiao.Migxin.FrontEnd.Lexical.Names;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Stmt;

internal record VarInitializeStmt(VarToken Var, NameToken Identifier, NameToken VarType, MigxinExpr Expr)
    : MigxinStmt($"var {Identifier.Text} : {VarType.Text} = {Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Identifier, VarType, Expr];
    
}