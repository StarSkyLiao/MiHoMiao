using System.Reflection;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitPow(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            Type rawType = resultType;
            if (resultType != typeof(double) && resultType != typeof(float)) resultType = typeof(double);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            Delegate method = (resultType == typeof(double)) ? Math.Pow : MathF.Pow;
            MigxnContext.EmitCode(new OpCall(method.Method));
            if (rawType != resultType) MigxnContext.EmitCode(new OpCast(resultType, rawType));
            return rawType;
        }
        
        // 情况 2: 检查自定义 operator *
        MethodInfo? customMethod = leftType.GetMethod("op_Power", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        return typeof(void);
    }
}