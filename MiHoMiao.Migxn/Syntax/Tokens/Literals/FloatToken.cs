using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Tokens.Literals;

public record FloatToken(int Position, ReadOnlyMemory<char> Text, double Value) : LiteralToken(Position, Text)
{
    
    internal override Type ExpressionType(MigxnContext context) => typeof(double);
    
    internal override void EmitCode(MigxnContext context, ILGenerator generator) 
        => generator.Emit(OpCodes.Ldc_R8, Value);

}