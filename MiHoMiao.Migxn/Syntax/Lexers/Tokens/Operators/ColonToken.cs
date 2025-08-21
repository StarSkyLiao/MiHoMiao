using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

internal record ColonToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken
{
    public static string UniqueName => ":";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new ColonToken(index, position);
    
}