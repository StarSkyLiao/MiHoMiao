using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    /// <summary>
    /// 处理赋值语句
    /// </summary>
    public override Type VisitAssignStmt(AssignStmtContext context)
    {
        // 先计算右侧表达式
        Type exprType = Visit(context.Right);
        if (exprType == typeof(void))
        {
            string message = $"Type of right expression \"{context.Right.GetText()}\" should not be void!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Assign().Symbol, message));
            return typeof(void);
        }
        MigxnVariable? item = VisitWriter(context.Left);
        if (item is { IsWritable: true })
        {
            if (item.Type != exprType) MigxnContext.EmitCode(new OpCast(exprType, item.Type));
            MigxnContext.EmitCode(new OpStVar(item.Name));
        }
        else
        {
            string message = $"Left expression \"{context.Left.GetText()}\" should be writeable!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Assign().Symbol, message));
        }
        return typeof(void);
    }
    
}