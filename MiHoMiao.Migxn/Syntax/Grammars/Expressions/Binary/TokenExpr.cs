using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

public record TokenExpr(LiteralToken Token) : MigxnExpr(Token.Text, Token.Index, Token.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [Token];
}