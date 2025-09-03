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
    public override Type VisitAndOrExpr(AndOrExprContext context)
    {
        // 先访问左子节点和右子节点，确保操作数先入栈
        Type leftType = Visit(context.Left);
        int leftTail = MigxnContext.AllMembers[^1].Codes.Count;
        Type rightType = Visit(context.Right);

        // 根据操作符类型将对应操作符推入栈
        Type resultType = context.op.Type switch
        {
            MigxnLiteral.And => EmitAnd(leftType, rightType, leftTail, context.Start),
            MigxnLiteral.Or => EmitOr(leftType, rightType, leftTail, context.Start),
            _ => throw new UnreachableException("Unknown binary operator")
        };
        if (resultType != typeof(void)) return resultType;
        string message = $"There is no \"{context.op.Text}\" operation between {leftType.Name} and {rightType.Name}!";
        MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Start, message));
        return resultType;
    }
    
}