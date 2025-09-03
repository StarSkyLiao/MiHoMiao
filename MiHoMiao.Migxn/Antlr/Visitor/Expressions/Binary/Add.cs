using System.Reflection;
using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitAdd(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 两个类型存在 string
        if (leftType == typeof(string) || rightType == typeof(string))
        {
            Type resultType = typeof(string);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            Func<string, string, string> func = string.Concat;
            MigxnContext.EmitCode(new OpCall(func.Method));
            return resultType;
        }

        // 情况 2: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpAdd());
            return resultType;
        }

        // 情况 3: 检查自定义 operator +
        MethodInfo? customMethod = leftType.GetMethod("op_Addition", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        return typeof(void);
    }
}