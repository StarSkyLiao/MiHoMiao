using System.Reflection;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Compare;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
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
}