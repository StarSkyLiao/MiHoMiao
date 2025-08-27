using MiHoMiao.Migxin.Syntax.Grammar.Expr;
using MiHoMiao.Migxin.Syntax.Lexical.Keywords.Core;
using MiHoMiao.Migxin.Syntax.Lexical.Names;

namespace MiHoMiao.Migxin.Syntax.Grammar.Stmt;

internal record VarAutoInitStmt(VarToken Var, NameToken Identifier, MigxinExpr Expr)
    : MigxinStmt($"var {Identifier.Text} = {Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Identifier, Expr];
    
}