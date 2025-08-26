using MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Symbol;

internal class MulSymbol : IBinarySymbol
{
    public uint Priority => 4;
    public string Text => " * ";
    public static Type TokenType => typeof(MulToken);
    
    public static IBinarySymbol LoadSymbol() => new MulSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IBinarySymbol.SymbolTryMatch<MulSymbol>(left, migxinGrammar);
}