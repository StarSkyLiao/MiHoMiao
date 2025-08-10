using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record UeqToken(int Position) : Punctuation(Position, "!=".AsMemory()), IBinaryToken
{
    int IBinaryToken.BinaryOpPriority => 0;
    OpCode IBinaryToken.BinaryOpCode => OpCodes.Ceq;

    Type IBinaryToken.BinaryType(int position, Type left, Type right) => typeof(bool);
    
}