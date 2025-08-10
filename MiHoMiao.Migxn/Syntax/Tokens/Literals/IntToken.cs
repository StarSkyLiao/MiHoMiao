using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Tokens.Literals;

public record IntToken(int Position, ReadOnlyMemory<char> Text, long Value) : LiteralToken(Position, Text)
{
    
    internal override Type ExpressionType(MigxnContext context) => typeof(long);
    
    internal override void EmitCode(MigxnContext context, ILGenerator generator)
    {
        switch (Value)
        {
            case 0: generator.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1: generator.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2: generator.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3: generator.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4: generator.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5: generator.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6: generator.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7: generator.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8: generator.Emit(OpCodes.Ldc_I4_8);
                break;
            case 9: generator.Emit(OpCodes.Ldc_I4_M1);
                break;
            case > short.MinValue and < short.MaxValue: 
                generator.Emit(OpCodes.Ldc_I4_S, (short)Value);
                break;
            case > int.MinValue and < int.MaxValue: 
                generator.Emit(OpCodes.Ldc_I4, (int)Value);
                break;
            default: generator.Emit(OpCodes.Ldc_I8, Value);
                break;
        }
    }
}