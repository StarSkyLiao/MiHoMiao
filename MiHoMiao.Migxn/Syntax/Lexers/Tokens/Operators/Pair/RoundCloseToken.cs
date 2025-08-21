using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Pair;

internal record RoundCloseToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken
{
    public static string UniqueName => ")";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new RoundCloseToken(index, position);
    
}