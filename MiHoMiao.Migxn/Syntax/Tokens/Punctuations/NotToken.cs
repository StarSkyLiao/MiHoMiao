using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record NotToken(int Position) : Punctuation(Position, "!".AsMemory()), IUnaryToken
{
    OpCode IUnaryToken.UnaryOpCode => OpCodes.Not;
}