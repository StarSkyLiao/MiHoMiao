using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record GotoStmt(GotoToken Goto, SymbolToken Identifier) 
    : MigxnStmt($"goto {Identifier.Text}".AsMemory(), Goto.Index, Goto.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Goto, Identifier];

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => [new OpGoto(Identifier.Text)];

}