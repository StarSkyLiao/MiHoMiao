using MiHoMiao.Migxin.Compiler.Lexical.Keywords.Core;
using MiHoMiao.Migxin.Compiler.Lexical.Names;

namespace MiHoMiao.Migxin.Compiler.Syntax.Stmt;

internal record VarDeclareStmt(VarToken Var, NameToken Identifier, NameToken VarType) 
    : MigxinStmt($"var {Identifier.Text} : {VarType.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxinNode> Children() => [Var, Identifier, VarType];

}