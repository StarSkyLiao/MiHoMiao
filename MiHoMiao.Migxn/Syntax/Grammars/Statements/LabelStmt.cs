using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record LabelStmt(LabelToken Label, SymbolToken Identifier) 
    : MigxnStmt($"label {Identifier.Text}:".AsMemory(), Label.Index, Label.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Label, Identifier];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => [new OpLabel(Identifier.Text)];

}