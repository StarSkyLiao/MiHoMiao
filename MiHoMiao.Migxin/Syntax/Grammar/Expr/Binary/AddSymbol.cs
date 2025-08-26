using MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;

internal class AddSymbol : IOperatorSymbol
{
    public uint Priority => 5;
    public string Text => " + ";
    public static Type TokenType => typeof(AddToken);
    
    public static IOperatorSymbol LoadSymbol() => new AddSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IOperatorSymbol.SymbolTryMatch<AddSymbol>(left, migxinGrammar);
}