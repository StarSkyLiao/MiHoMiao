using System.Diagnostics;
using System.Reflection;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    
    /// <summary>
    /// 处理二元表达式（加、减、乘、除、取模、幂）
    /// </summary>
    public override Type VisitBinaryExpr(BinaryExprContext context)
    {
        // 先访问左子节点和右子节点，确保操作数先入栈
        Type leftType = Visit(context.Left);
        int leftTail = MigxnContext.AllMembers[^1].Codes.Count;
        Type rightType = Visit(context.Right);

        // 根据操作符类型将对应操作符推入栈
        Type resultType = context.op.Type switch
        {
            MigxnLiteral.Add => EmitAdd(leftType, rightType, leftTail),
            MigxnLiteral.Sub => EmitSub(leftType, rightType, leftTail),
            MigxnLiteral.Mul => EmitMul(leftType, rightType, leftTail),
            MigxnLiteral.Div => EmitDiv(leftType, rightType, leftTail),
            MigxnLiteral.Rem => EmitRem(leftType, rightType, leftTail),
            MigxnLiteral.Pow => EmitPow(leftType, rightType, leftTail),
            _ => throw new UnreachableException("Unknown binary operator")
        };
        if (resultType != typeof(void)) return resultType;
        string message = $"There is no \"{context.op.Text}\" method between {leftType.Name} and {rightType.Name}!";
        MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Start, message));
        return resultType;
    }
    
    private Type EmitAdd(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 两个类型都是 string
        if (leftType == typeof(string) && rightType == typeof(string))
        {
            Func<string, string, string> func = string.Concat;
            MigxnContext.EmitCode(new OpCall(func.Method));
            return typeof(string);
        }
        // 情况 2: 左操作数是 string，右操作数调用 ToString
        if (leftType == typeof(string))
        {
            MethodInfo toString = rightType.GetMethod(nameof(ToString), Type.EmptyTypes)!;
            MigxnContext.EmitCode(new OpCall(toString));
            Func<string, string, string> func = string.Concat;
            MigxnContext.EmitCode(new OpCall(func.Method));
            return typeof(string);
        }
        // 情况 3: 右操作数是 string，左操作数调用 ToString
        if (rightType == typeof(string))
        {
            MethodInfo toString = leftType.GetMethod(nameof(ToString), Type.EmptyTypes)!;
            MigxnContext.InsertEmitCode(leftTail, new OpCall(toString));
            Func<string, string, string> func = string.Concat;
            MigxnContext.EmitCode(new OpCall(func.Method));
            return typeof(string);
        }
        // 情况 4: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpAdd());
            return resultType;
        }
        
        // 情况 5: 检查自定义 operator +
        MethodInfo? customMethod = leftType.GetMethod("op_Addition", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        return typeof(void);
    }
    
    private Type EmitSub(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpSub());
            return resultType;
        }
        
        // 情况 2: 检查自定义 operator -
        MethodInfo? customMethod = leftType.GetMethod("op_Subtraction", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        return typeof(void);
    }
    
    private Type EmitMul(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpMul());
            return resultType;
        }
        
        // 情况 2: 检查自定义 operator *
        MethodInfo? customMethod = leftType.GetMethod("op_Multiply", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        return typeof(void);
    }
    
    private Type EmitDiv(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpMul());
            return resultType;
        }
        
        // 情况 2: 检查自定义 operator *
        MethodInfo? customMethod = leftType.GetMethod("op_Division", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        return typeof(void);
    }
    
    private Type EmitRem(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpMul());
            return resultType;
        }
        
        // 情况 2: 检查自定义 operator *
        MethodInfo? customMethod = leftType.GetMethod("op_Modulus", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        return typeof(void);
    }
    
    private Type EmitPow(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理 数字 类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            Type rawType = resultType;
            if (resultType != typeof(double) && resultType != typeof(float)) resultType = typeof(float);
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