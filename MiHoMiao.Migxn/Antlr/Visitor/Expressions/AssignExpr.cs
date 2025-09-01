using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnCommonParser
{
    
    /// <summary>
    /// 处理赋值表达式
    /// </summary>
    public override Type? VisitAssignExpr(AssignExprContext context)
    {
        // 先计算右侧表达式
        Visit(context.Right);
        
        MigxnVariable? item = VisitWriter(context.Left);
        if (item is { IsWritable: true }) Codes.Add(new OpStVar(item.Name));
        else Exceptions.Add(MigxnDiagnostic.Create(context.Assign().Symbol, $"Left expression \"{context.Left.GetText()}\" should be writeable!"));
        return null;
    }
    
}