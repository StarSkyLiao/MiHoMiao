namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Comments;

public record SingleLineComment(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position) : MigxnToken(Text, Index, Position)
{
    
}