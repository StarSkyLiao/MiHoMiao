using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

internal record DefaultExpr(DefaultToken DefaultToken) : MigxnExpr(DefaultToken.Text, DefaultToken.Index, DefaultToken.Position)
{
    public override Type ExprType(MigxnContext context) => typeof(object);
    
    internal override IEnumerable<MigxnNode> Children() => [DefaultToken];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => DefaultToken.AsOpCodes();
    
}