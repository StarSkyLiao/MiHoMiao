using MiHoMiao.Migxn.CodeAnalysis.Parser;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

/// <summary>
/// 比较的二元运算符
/// </summary>
internal interface ICompareBinary<T> : IBinaryToken where T : ICompareBinary<T>
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
        if (typeLeft == typeRight) return typeof(bool);
        ErrorTypeExpr exception = new ErrorTypeExpr(left, typeLeft, typeRight, typeof(T).Name);
        context.MigxnParser.Exceptions.Add(exception);
        return typeof(void);
    }

}