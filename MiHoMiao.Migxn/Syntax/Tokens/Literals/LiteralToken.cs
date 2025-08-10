using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Node;

namespace MiHoMiao.Migxn.Syntax.Tokens.Literals;

public abstract record LiteralToken(int Position, ReadOnlyMemory<char> Text)
    : MigxnToken(Position, Text)
{
    protected override IEnumerable<MigxnNode> Children() => [];

    internal abstract Type ExpressionType(MigxnContext context);

}