namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record DoubleToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    
}