using MiHoMiao.Migxn.CodeAnalysis.Parser;
using MiHoMiao.Migxn.Reflection;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

/// <summary>
/// 使用移位数学运算的二元运算符
/// </summary>
internal interface IIntShiftBinary<T> : IBinaryToken where T : IIntShiftBinary<T>
{
    static abstract MigxnOpCode Operator { get; }
    
    IEnumerable<MigxnOpCode> IBinaryToken.BinaryOp(MigxnExpr left, MigxnExpr right, MigxnContext context)
    {
        return [..left.AsOpCodes(context), ..right.AsOpCodes(context), T.Operator];
    }
    Type IBinaryToken.BinaryType(MigxnExpr left, MigxnExpr right, MigxnContext context)
    {
        Type typeLeft = left.ExprType(context);
        Type typeRight = right.ExprType(context);
        Type? result = ReflectTool.IntShiftType(typeLeft, typeRight);
        if (result is not null) return result;
        ErrorTypeExpr exception = new ErrorTypeExpr(left, typeLeft, typeRight, typeof(T).Name);
        context.MigxnParser.Exceptions.Add(exception);
        return typeof(void);
    }
}