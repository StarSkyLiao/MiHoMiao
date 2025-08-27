using MiHoMiao.Migxin.FrontEnd.Lexical.Operators.Assign;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Prefix;

internal class IncSymbol : IPrefixSymbol
{
    public uint Priority => 1;
    public string Text => "++";
    public static Type TokenType => typeof(IncToken);
    
    public static IOperatorSymbol LoadSymbol() => new IncSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IPrefixSymbol.SymbolTryMatch<IncSymbol>(left, migxinGrammar);
}