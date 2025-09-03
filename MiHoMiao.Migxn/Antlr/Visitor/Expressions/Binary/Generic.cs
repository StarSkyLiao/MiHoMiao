using System.Diagnostics;
using System.Reflection;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitBinary<T>(Type leftType, Type rightType, int leftTail, string custom) where T : MigxnOpCode, new()
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new T());
            return resultType;
        }

        // 情况 2: 检查自定义 operator
        MethodInfo? customMethod = leftType.GetMethod(custom, PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        return typeof(void);
    }
    
    private Type EmitCompare<T>(Type leftType, Type rightType, int leftTail, string custom) where T : MigxnOpCode, new()
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new T());
            return typeof(bool);
        }

        // 情况 2: 检查自定义 operator
        MethodInfo? customMethod = leftType.GetMethod(custom, PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        return typeof(void);
    }
    
    private const BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static;

    private static bool IsNumericType(Type type)
    {
        return type == typeof(char) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double);
    }

    private static Type GetNumericResultType(Type leftType, Type rightType)
    {
        if (leftType == typeof(double) || rightType == typeof(double)) return typeof(double);
        if (leftType == typeof(float) || rightType == typeof(float)) return typeof(float);
        if (leftType == typeof(long) || rightType == typeof(long)) return typeof(long);
        if (leftType == typeof(int) || rightType == typeof(int)) return typeof(int);
        if (leftType == typeof(char) || rightType == typeof(char)) return typeof(int);
        throw new UnreachableException();
    }
}