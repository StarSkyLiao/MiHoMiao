using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Node.Expressions;

public abstract record MigxnExpr(ReadOnlyMemory<char> Text, bool IsInCurly)
    : MigxnNode(IsInCurly ? $"({Text})".AsMemory() : Text)
{
    public abstract Type ExpressionType(MigxnContext context);
}