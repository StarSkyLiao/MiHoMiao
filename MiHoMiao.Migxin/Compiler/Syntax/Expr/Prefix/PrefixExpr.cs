using MiHoMiao.Migxin.Compiler.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.Compiler.Syntax.Expr.Prefix;

internal record PrefixExpr(IOperatorSymbol OperatorSymbol, MigxinExpr Right)
    : MigxinExpr($"{OperatorSymbol.Text}{Right.Text}".AsMemory(), Right.Index, Right.Position)
{
    public IOperatorSymbol OperatorSymbol { get; init; } = OperatorSymbol;
    internal override IEnumerable<MigxinTree> Children() => [Right];


}