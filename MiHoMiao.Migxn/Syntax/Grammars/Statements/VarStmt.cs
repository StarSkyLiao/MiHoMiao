using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record VarStmt(VarToken Var, SymbolToken Identifier, MigxnExpr Expr) 
    : MigxnStmt($"var {Identifier.Text} = {Expr.Text.ToString()}".AsMemory(), 
        Var.Index, Var.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Var, Identifier, Expr];

    public Type VarType { get; set; } = typeof(object);

    protected override string SelfString() =>
        $"{GetType().Name} (Line: {Position.Line}, Column: {Position.Column}): " +
        $"var {Identifier.Text} : {VarType.Name} = {Expr.Text.ToString()}";

}