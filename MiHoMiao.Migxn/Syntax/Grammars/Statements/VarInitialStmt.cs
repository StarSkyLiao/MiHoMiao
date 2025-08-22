using MiHoMiao.Migxn.CodeAnalysis.Parser;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Store;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record VarInitialStmt(VarToken Var, SymbolToken Identifier, SymbolToken? VarType, MigxnExpr Expr)
    : MigxnStmt($"var {Identifier.Text} : {VarType?.Text.ToString() ?? "any"} = {Expr.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxnNode> Children() => VarType != null ? [Var, Identifier, VarType, Expr] : [Var, Identifier, Expr];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        try
        {
            Type exprType = Expr.ExprType(context);
            if (VarType != null)
            {
                string type = VarType.Text.ToString();
                Type? baseType = context.ParseType(type);
                if (baseType is null)
                {
                    UndefinedType exception = new UndefinedType(Identifier.Position with { Column = Identifier.NextColumn }, type);
                    context.MigxnParser.Exceptions.Add(exception);
                    return [];
                }
                if (baseType != exprType)
                {
                    context.MigxnParser.Exceptions.Add(new ErrorTypeAssign(Identifier, exprType, baseType));
                    return [];
                }
            }

            MigxnVariable variable = new MigxnVariable(Identifier.Text, exprType);
            context.Variables.DeclareVariable(Identifier.Text.ToString(), variable);
        }
        catch (Exception ex)
        {
            context.MigxnParser.Exceptions.Add(ex);
        }

        return [..Expr.AsOpCodes(context), new OpStVar(Identifier.Text)];
    }

}