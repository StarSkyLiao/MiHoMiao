using MiHoMiao.Migxin.Compiler.Lexical.Operators.Assign;

namespace MiHoMiao.Migxin.Compiler.Syntax.Expr.Binary;

internal class AssignSymbol : IOperatorSymbol
{
    public uint Priority => 16;
    public string Text => " = ";
    public static Type TokenType => typeof(AssignToken);
    
    public static IOperatorSymbol LoadSymbol() => new AssignSymbol();

    public static MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar)
        => IOperatorSymbol.TryMatchSymbol<AssignSymbol>(left, migxinGrammar);
}