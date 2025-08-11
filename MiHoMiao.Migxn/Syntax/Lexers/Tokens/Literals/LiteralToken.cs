namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record LiteralToken(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position) : MigxnToken(Text, Index, Position)
{
    
}