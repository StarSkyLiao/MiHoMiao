using MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;
using MiHoMiao.Migxin.Syntax.Lexical.Operators.Advance;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Suffix;

internal class NotNullSymbol : ISuffixSymbol
{
    public uint Priority => 0;
    public string Text => "!";
    public static Type TokenType => typeof(ExclamationToken);
    
    public static IOperatorSymbol LoadSymbol() => new NotNullSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => ISuffixSymbol.SymbolTryMatch<NotNullSymbol>(left, migxinGrammar);
}