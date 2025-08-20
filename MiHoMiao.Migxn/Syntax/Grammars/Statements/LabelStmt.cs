using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Flow;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record LabelStmt(LabelToken Label, SymbolToken Identifier) 
    : MigxnStmt($"label {Identifier.Text}:".AsMemory(), Label.Index, Label.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Label, Identifier];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => [new OpLabel(Identifier.Text)];

}