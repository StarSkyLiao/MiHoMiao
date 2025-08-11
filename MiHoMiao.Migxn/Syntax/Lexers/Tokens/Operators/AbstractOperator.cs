using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

public record AbstractOperator(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position)
    : LiteralToken(Index, Text, Position)
{
    
}