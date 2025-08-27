using MiHoMiao.Migxin.Syntax.Lexical.Keywords.Core;
using MiHoMiao.Migxin.Syntax.Lexical.Names;

namespace MiHoMiao.Migxin.Syntax.Grammar.Stmt;

internal record VarDeclareStmt(VarToken Var, NameToken Identifier, NameToken VarType) 
    : MigxinStmt($"var {Identifier.Text} : {VarType.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Identifier, VarType];

}