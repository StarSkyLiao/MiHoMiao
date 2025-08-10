using System.Reflection.Emit;
using MiHoMiao.Migxn.Exceptions.Grammar;

namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public record AddToken(int Position) : Punctuation(Position, "+".AsMemory()), IBinaryToken, IUnaryToken
{
    int IBinaryToken.BinaryOpPriority => 3;
    OpCode IBinaryToken.BinaryOpCode => OpCodes.Add;
    OpCode IUnaryToken.UnaryOpCode => OpCodes.Nop;
    
    // 预计算 <Type,int> 索引，越小优先级越高
    private static readonly Dictionary<Type,int> s_Dictionary = new Dictionary<Type, int>
    {
        [typeof(string)] = 0,
        [typeof(double)] = 1,
        [typeof(long)]   = 2,
        [typeof(char)]   = 3,
    };

    Type IBinaryToken.BinaryType(int position, Type left, Type right) =>
        s_Dictionary.TryGetValue(left, out var idxL) && s_Dictionary.TryGetValue(right, out var idxR)
            ? idxL <= idxR ? left : right
            : throw new TypeNotFitException(position, left, right);
}