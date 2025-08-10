namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record FldToken(int Position) : Punctuation(Position, ".".AsMemory())
{
    
}