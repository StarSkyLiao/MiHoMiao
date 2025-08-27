using MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;

internal class DotSymbol : IOperatorSymbol
{
    public uint Priority => 0;
    public string Text => ".";
    public static Type TokenType => typeof(DotToken);
    
    public static IOperatorSymbol LoadSymbol() => new DotSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IOperatorSymbol.TryMatchSymbol<DotSymbol>(left, migxinGrammar);
}