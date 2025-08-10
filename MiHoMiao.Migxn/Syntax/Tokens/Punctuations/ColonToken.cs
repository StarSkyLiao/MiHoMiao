namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record ColonToken(int Position) : Punctuation(Position, ":".AsMemory());