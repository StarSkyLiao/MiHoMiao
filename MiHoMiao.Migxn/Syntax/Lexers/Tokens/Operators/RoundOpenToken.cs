using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

public record RoundOpenToken(int Index, (int Line, int Column) Position)
    : AbstractOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken
{
    public static string UniqueName => "(";

    public static AbstractOperator Create(int index, (int Line, int Column) position) => new RoundOpenToken(index, position);
    
}