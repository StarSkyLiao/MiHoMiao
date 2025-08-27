using MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;

internal class MulSymbol : IOperatorSymbol
{
    public uint Priority => 4;
    public string Text => " * ";
    public static Type TokenType => typeof(MulToken);
    
    public static IOperatorSymbol LoadSymbol() => new MulSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IOperatorSymbol.TryMatchSymbol<MulSymbol>(left, migxinGrammar);
}