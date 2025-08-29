using System.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeGen.Algorithm;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnStmt;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnLanguage
{
    
    /// <summary>
    /// 处理二元表达式（加、减、乘、除、取模、幂）
    /// </summary>
    public override Type? VisitBinaryExpr(BinaryExprContext context)
    {
        // 先访问左子节点和右子节点，确保操作数先入栈
        Visit(context.Left); // 左操作数
        Visit(context.Right); // 右操作数

        // 根据操作符类型将对应操作符推入栈
        Codes.Add(context.op.Type switch
        {
            MigxnLiteral.Add => new OpAdd(),
            MigxnLiteral.Sub => new OpSub(),
            MigxnLiteral.Mul => new OpMul(),
            MigxnLiteral.Div => new OpDiv(),
            MigxnLiteral.Rem => new OpRem(),
            MigxnLiteral.Pow => new OpPow(),
            _ => throw new UnreachableException("Unknown binary operator")
        });

        return null;
    }
    
}