using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record VarStmt(VarToken Var, SymbolToken Identifier, ColonToken? Colon, SymbolToken? VarType, EqualToken? Equal, MigxnExpr? Expr) 
    : MigxnStmt($"var {Identifier.Text} : {VarType?.Text.ToString() ?? "any"} = {Expr?.Text.ToString() ?? "default"}".AsMemory(), 
        Var.Index, Var.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Var, Identifier, Colon, VarType, Equal, Expr];
}