namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Comments;

public abstract record IgnoredToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) : MigxnToken(Text, Index, Position)
{
    
}