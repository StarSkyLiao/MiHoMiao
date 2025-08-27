using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Grammar;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Suffix;

internal interface ISuffixSymbol : IOperatorSymbol
{
    
    protected new static MigxinResult<MigxinExpr> SymbolTryMatch<T>(MigxinExpr? left, MigxinGrammar migxinGrammar)
        where T : ISuffixSymbol
    {
        if (left == null) return new DiagnosticBag(new ShouldBe(migxinGrammar.Position, nameof(MigxinExpr)));
        IOperatorSymbol operatorSymbol = T.LoadSymbol();
        if (left is BinaryExpr leftBinary && leftBinary.OperatorSymbol.Priority <= operatorSymbol.Priority)
            left = BinaryExpr.CombineBinary(leftBinary.Left, leftBinary.OperatorSymbol, left);

        return new SuffixExpr(left, operatorSymbol);
    }
    
}