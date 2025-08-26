using MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Symbol;

internal class AddSymbol : IBinarySymbol
{
    public uint Priority => 5;
    public string Text => " + ";
    public static Type TokenType => typeof(AddToken);
    
    public static IBinarySymbol LoadSymbol() => new AddSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IBinarySymbol.SymbolTryMatch<AddSymbol>(left, migxinGrammar);
}