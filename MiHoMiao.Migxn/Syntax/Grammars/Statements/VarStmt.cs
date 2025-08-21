using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Store;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record VarStmt(VarToken Var, SymbolToken Identifier, MigxnExpr Expr) 
    : MigxnStmt($"var {Identifier.Text} = {Expr.Text.ToString()}".AsMemory(), 
        Var.Index, Var.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Var, Identifier, Expr];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => Expr.AsOpCodes().Concat([new OpStVar(Identifier.Text)]);

    public Type VarType { get; set; } = typeof(object);

    protected override string SelfString() =>
        $"{GetType().Name} (Line: {Position.Line}, Column: {Position.Column}): " +
        $"var {Identifier.Text} : {VarType.Name} = {Expr.Text.ToString()}";

}