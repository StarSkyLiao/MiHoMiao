using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    
    /// <summary>
    /// 处理赋值表达式
    /// </summary>
    public MigxnVariable? VisitWriter(ExpressionContext context)
    {
        if (context is SingleExprContext single)
        {
            Result<MigxnVariable>? result = single.Value.Type switch
            {
                MigxnLiteral.Name => MigxnContext.MigxnScope.LoadVariable(single.Value.Text),
                _ => default(Result<MigxnVariable>?)
            };
            if (result.HasValue)
            {
                if (result.Value.IsSuccess) return result.Value;
                MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(single.Value, result.Value.Exception!));
                return null;
            }
        }

        return null;
    }
    
}