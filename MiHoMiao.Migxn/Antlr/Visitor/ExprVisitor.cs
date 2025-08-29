using System.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Runtime;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnStmt;

namespace MiHoMiao.Migxn.Antlr.Visitor;

public class ExprVisitor(MigxnContext migxnContext) : MigxnStmtBaseVisitor<object>
{
    protected readonly MigxnContext MigxnContext = migxnContext;

    public override object VisitParenthesesExpr(ParenthesesExprContext context)
    {
        // 括号表达式只需递归访问内部表达式，无需额外操作
        return Visit(context.expression());
    }
    
    // 处理二元表达式（加、减、乘、除、取模、幂）
    public override object VisitBinaryExpr(BinaryExprContext context)
    {
        // 先访问左子节点和右子节点，确保操作数先入栈
        Visit(context.Left); // 左操作数
        Visit(context.Right); // 右操作数

        // 根据操作符类型将对应操作符推入栈
        MigxnContext.Codes.Add((context.op.Type) switch
        {
            MigxnLiteral.Add => new OpAdd(),
            MigxnLiteral.Sub => new OpSub(),
            MigxnLiteral.Mul => new OpMul(),
            MigxnLiteral.Div => new OpDiv(),
            MigxnLiteral.Rem => new OpRem(),
            MigxnLiteral.Pow => new OpPow(),
            _ => throw new UnreachableException("Unknown binary operator")
        });

        return null!; // 无需返回值
    }
    
    // 处理单值表达式（整数或浮点数）
    public override object VisitSingleExpr(SingleExprContext context)
    {
        string text = context.GetText();
        MigxnContext.Codes.Add((context.value.Type) switch
        {
            MigxnLiteral.Integer => new OpLdcInt(long.Parse(text)),
            MigxnLiteral.Float =>   new OpLdcFloat(double.Parse(text)),
            _ => throw new UnreachableException()
        });

        return null!;
    }
    
}