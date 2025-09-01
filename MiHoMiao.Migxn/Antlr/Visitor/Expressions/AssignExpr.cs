using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    /// <summary>
    /// 处理赋值语句
    /// </summary>
    public override Type? VisitAssignStmt(AssignStmtContext context)
    {
        // 先计算右侧表达式
        Visit(context.Right);
        
        MigxnVariable? item = VisitWriter(context.Left);
        if (item is { IsWritable: true }) MigxnContext.EmitCode(new OpStVar(item.Name));
        else MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Assign().Symbol, $"Left expression \"{context.Left.GetText()}\" should be writeable!"));
        return null;
    }
    
}