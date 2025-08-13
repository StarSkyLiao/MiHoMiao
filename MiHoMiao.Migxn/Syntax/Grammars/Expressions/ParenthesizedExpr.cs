using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public record ParenthesizedExpr(RoundOpenToken Left, MigxnExpr Content, RoundCloseToken Right) 
    : MigxnExpr($"({Content.Text})".AsMemory(), Left.Index, Left.Position)
{
    internal override IEnumerable<MigxnNode?> Children() => [Left, Content, Right];
}