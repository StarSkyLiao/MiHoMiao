namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record CurlyLeftToken(int Position) : Punctuation(Position, "{".AsMemory())
{
    
}