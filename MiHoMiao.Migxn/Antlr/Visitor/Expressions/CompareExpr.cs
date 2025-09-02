using System.Diagnostics;
using System.Reflection;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Compare;
using MiHoMiao.Migxn.CodeGen.Flow;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type VisitCompareExpr(CompareExprContext context)
    {
        // 先访问左子节点和右子节点，确保操作数先入栈
        Type leftType = Visit(context.Left);
        int leftTail = MigxnContext.AllMembers[^1].Codes.Count;
        Type rightType = Visit(context.Right);

        // 根据操作符类型将对应操作符推入栈
        Type resultType = context.op.Type switch
        {
            MigxnLiteral.Eql => EmitEql(leftType, rightType, leftTail),
            MigxnLiteral.Ueql => EmitNotEqual(leftType, rightType, leftTail),
            MigxnLiteral.Cgt => EmitCgt(leftType, rightType, leftTail),
            MigxnLiteral.Cge => EmitCge(leftType, rightType, leftTail),
            MigxnLiteral.Clt => EmitClt(leftType, rightType, leftTail),
            MigxnLiteral.Cle => EmitCle(leftType, rightType, leftTail),
            _ => throw new UnreachableException("Unknown binary operator")
        };
        if (resultType != typeof(void)) return resultType;
        string message = $"There is no \"{context.op.Text}\" method between {leftType.Name} and {rightType.Name}!";
        MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Start, message));
        return resultType;
    }
    
    private Type EmitEql(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理数字类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpCeq());
            return typeof(bool);
        }

        // 情况 2: 检查自定义 operator ==
        MethodInfo? customMethod = leftType.GetMethod("op_Equality", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }
        
        Func<object, object, bool> equals = Equals;
        customMethod = equals.Method;
        MigxnContext.EmitCode(new OpCall(customMethod));
        return customMethod.ReturnType;
    }
    
    private Type EmitNotEqual(Type leftType, Type rightType, int leftTail)
    {
        Type result = EmitEql(leftType, rightType, leftTail);
        if (result != typeof(void)) MigxnContext.EmitCode(new OpNeg());
        return result;
    }

    private Type EmitCgt(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理数字类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpCgt());
            return typeof(bool);
        }

        // 情况 2: 检查自定义 operator >
        MethodInfo? customMethod = leftType.GetMethod("op_GreaterThan", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        // 情况 3: 默认情况，返回 void 表示不支持
        return typeof(void);
    }

    private Type EmitClt(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理数字类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpClt());
            return typeof(bool);
        }

        // 情况 2: 检查自定义 operator <
        MethodInfo? customMethod = leftType.GetMethod("op_LessThan", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        // 情况 3: 默认情况，返回 void 表示不支持
        return typeof(void);
    }

    private Type EmitCge(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理数字类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpCge());
            return typeof(bool);
        }

        // 情况 2: 检查自定义 operator >=
        MethodInfo? customMethod = leftType.GetMethod("op_GreaterThanOrEqual", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        // 情况 3: 默认情况，返回 void 表示不支持
        return typeof(void);
    }

    private Type EmitCle(Type leftType, Type rightType, int leftTail)
    {
        // 情况 1: 处理数字类型
        if (IsNumericType(leftType) && IsNumericType(rightType))
        {
            Type resultType = GetNumericResultType(leftType, rightType);
            if (leftType != resultType) MigxnContext.InsertEmitCode(leftTail, new OpCast(leftType, resultType));
            if (rightType != resultType) MigxnContext.EmitCode(new OpCast(rightType, resultType));
            MigxnContext.EmitCode(new OpCle());
            return typeof(bool);
        }

        // 情况 2: 检查自定义 operator <=
        MethodInfo? customMethod = leftType.GetMethod("op_LessThanOrEqual", PublicStatic, [leftType, rightType]);
        if (customMethod != null)
        {
            MigxnContext.EmitCode(new OpCall(customMethod));
            return customMethod.ReturnType;
        }

        // 情况 3: 默认情况，返回 void 表示不支持
        return typeof(void);
    }

}