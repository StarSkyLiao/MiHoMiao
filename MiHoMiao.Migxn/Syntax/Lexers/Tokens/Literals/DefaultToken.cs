namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record DefaultToken(int Index, (int Line, int Column) Position) : LiteralToken("default".AsMemory(), Index, Position)
{
    
}