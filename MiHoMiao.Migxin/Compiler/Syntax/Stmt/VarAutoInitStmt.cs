using MiHoMiao.Migxin.Compiler.Lexical.Keywords.Core;
using MiHoMiao.Migxin.Compiler.Lexical.Names;
using MiHoMiao.Migxin.Compiler.Syntax.Expr;

namespace MiHoMiao.Migxin.Compiler.Syntax.Stmt;

internal record VarAutoInitStmt(VarToken Var, NameToken Identifier, MigxinExpr Expr)
    : MigxinStmt($"var {Identifier.Text} = {Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Identifier, Expr];
    
}