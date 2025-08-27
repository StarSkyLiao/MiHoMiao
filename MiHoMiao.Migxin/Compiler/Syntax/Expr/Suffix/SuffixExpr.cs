using MiHoMiao.Migxin.Compiler.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.Compiler.Syntax.Expr.Suffix;

internal record SuffixExpr(MigxinExpr Left, IOperatorSymbol OperatorSymbol)
    : MigxinExpr($"{Left.Text}{OperatorSymbol.Text}".AsMemory(), Left.Index, Left.Position)
{
    public IOperatorSymbol OperatorSymbol { get; init; } = OperatorSymbol;
    internal override IEnumerable<MigxinTree> Children() => [Left];


}