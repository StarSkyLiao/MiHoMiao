using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public record DefaultExpr(DefaultToken DefaultToken) : MigxnExpr(DefaultToken.Text, DefaultToken.Index, DefaultToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [DefaultToken];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => DefaultToken.AsOpCodes();
}