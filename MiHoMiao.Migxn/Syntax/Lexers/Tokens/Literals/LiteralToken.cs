namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public abstract record LiteralToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)
{
    
}