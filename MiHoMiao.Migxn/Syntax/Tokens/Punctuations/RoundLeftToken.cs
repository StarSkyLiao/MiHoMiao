namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record RoundLeftToken(int Position) : Punctuation(Position, "(".AsMemory())
{
    
}