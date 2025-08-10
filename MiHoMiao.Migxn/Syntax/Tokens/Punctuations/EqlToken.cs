namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record EqlToken(int Position) : Punctuation(Position, "=".AsMemory())
{
    
}