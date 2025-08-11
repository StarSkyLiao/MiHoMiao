namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record SymbolToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{

}