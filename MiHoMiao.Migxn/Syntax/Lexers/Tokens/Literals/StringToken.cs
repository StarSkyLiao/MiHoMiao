namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record StringToken(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position) : LiteralToken(Index, Text, Position)
{
    
}