namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record CurlyRightToken(int Position) : Punctuation(Position, "}".AsMemory())
{
    
}