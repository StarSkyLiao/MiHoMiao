using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

public interface IUnaryToken
{
    public abstract OpCode UnaryOpCode { get; }
}