using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public interface IBinaryToken
{
    
    public abstract int BinaryOpPriority { get; }
    public abstract OpCode BinaryOpCode { get; }
    
    public abstract Type BinaryType(int position, Type left, Type right);
    
}