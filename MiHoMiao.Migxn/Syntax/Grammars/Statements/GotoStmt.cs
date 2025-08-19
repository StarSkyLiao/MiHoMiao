using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record GotoStmt(GotoToken Goto, SymbolToken Identifier) 
    : MigxnStmt($"goto {Identifier.Text}".AsMemory(), Goto.Index, Goto.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Goto, Identifier];
    
}