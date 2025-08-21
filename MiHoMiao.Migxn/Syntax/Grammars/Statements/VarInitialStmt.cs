using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Store;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record VarInitialStmt(VarToken Var, SymbolToken Identifier, MigxnExpr Expr) 
    : MigxnStmt($"var {Identifier.Text} = {Expr}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Var, Identifier, Expr];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        try
        {
            MigxnVariable variable = new MigxnVariable(Identifier.Text, Expr.ExprType(context));
            context.Variables.DeclareVariable(Identifier.Text.ToString(), variable);
        }
        catch (Exception ex)
        {
            context.MigxnParser.Exceptions.Add(ex);
        }

        return [..Expr.AsOpCodes(context), new OpStVar(Identifier.Text)];
    }

    protected override string SelfString() =>
        $"{GetType().Name} (Line: {Position.Line}, Column: {Position.Column}): " +
        $"var {Identifier.Text} : any = {Expr.Text}";

}