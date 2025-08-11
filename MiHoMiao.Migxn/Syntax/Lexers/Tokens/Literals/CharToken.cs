namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record CharToken(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position) : LiteralToken(Index, Text, Position)
{
    
}