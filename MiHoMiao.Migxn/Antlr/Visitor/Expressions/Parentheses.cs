using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type? VisitParenthesesExpr(ParenthesesExprContext context)
    {
        // 括号表达式只需递归访问内部表达式，无需额外操作
        return Visit(context.expression());
    }
}