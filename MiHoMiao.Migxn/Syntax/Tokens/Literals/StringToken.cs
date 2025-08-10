using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Tokens.Literals;

public record StringToken(int Position, ReadOnlyMemory<char> Text, string Value) : LiteralToken(Position, Text)
{
    
    internal override Type ExpressionType(MigxnContext context) => typeof(string);

    internal override void EmitCode(MigxnContext context, ILGenerator generator)
        => generator.Emit(OpCodes.Ldstr, Value);
    
}