using MiHoMiao.Migxin.Compiler.Lexical.Operators.Advance;
using MiHoMiao.Migxin.Compiler.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.Compiler.Syntax.Expr.Suffix;

internal class NotNullSymbol : ISuffixSymbol
{
    public uint Priority => 0;
    public string Text => "!";
    public static Type TokenType => typeof(ExclamationToken);
    
    public static IOperatorSymbol LoadSymbol() => new NotNullSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => ISuffixSymbol.SymbolTryMatch<NotNullSymbol>(left, migxinGrammar);
}