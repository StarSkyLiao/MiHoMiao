using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Grammar;
using MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Prefix;

internal interface IPrefixSymbol : IOperatorSymbol
{
    
    protected new static MigxinResult<MigxinExpr> SymbolTryMatch<T>(MigxinExpr? left, MigxinGrammar migxinGrammar)
        where T : IPrefixSymbol
    {
        if (left != null) return new DiagnosticBag(new ShouldBe(migxinGrammar.Position, nameof(IOperatorSymbol)));
        MigxinResult<MigxinExpr> right = MigxinExpr.TryParse(migxinGrammar);
        if (right.IsSuccess) return BinaryExpr.CombineBinary(left, T.LoadSymbol(), right.Result!);
        right.Exception!.Attach(new ShouldBe(migxinGrammar.Position, nameof(MigxinExpr)));
        return right;
    }
    
}