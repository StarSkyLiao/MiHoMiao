using MiHoMiao.Migxn.CodeAnalysis.Parser;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record VarDeclareStmt(VarToken Var, SymbolToken Identifier, SymbolToken VarType) 
    : MigxnStmt($"var {Identifier.Text} : {VarType.Text}".AsMemory(), Var.Index, Var.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Var, Identifier, VarType];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        try
        {
            string type = VarType.Text.ToString();
            Type? baseType = context.ParseType(type);
            if (baseType is null)
            {
                UndefinedType exception = new UndefinedType(Identifier.Position with { Column = Identifier.NextColumn }, type);
                context.MigxnParser.Exceptions.Add(exception);
                return [];
            }
            MigxnVariable variable = new MigxnVariable(Identifier.Text, baseType);
            context.Variables.DeclareVariable(Identifier.Text.ToString(), variable);
        }
        catch (Exception ex)
        {
            context.MigxnParser.Exceptions.Add(ex);
        }

        return [];
    }

    protected override string SelfString() =>
        $"{GetType().Name} (Line: {Position.Line}, Column: {Position.Column}): " +
        $"var {Identifier.Text} : {VarType} = undefined";

}