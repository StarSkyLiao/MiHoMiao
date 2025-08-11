using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

public record TokenExpr(MigxnToken Token) : MigxnExpr(Token.Text, Token.Index, Token.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Token];
}