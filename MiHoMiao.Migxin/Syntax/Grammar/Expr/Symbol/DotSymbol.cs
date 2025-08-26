using MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Symbol;

internal class DotSymbol : IBinarySymbol
{
    public uint Priority => 0;
    public string Text => ".";
    public static Type TokenType => typeof(DotToken);
    
    public static IBinarySymbol LoadSymbol() => new DotSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IBinarySymbol.SymbolTryMatch<DotSymbol>(left, migxinGrammar);
}