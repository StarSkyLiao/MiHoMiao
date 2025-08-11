namespace MiHoMiao.Migxn.Syntax.Lexers;

public record DefaultToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) : MigxnToken(Text, Index, Position)
{
    
}