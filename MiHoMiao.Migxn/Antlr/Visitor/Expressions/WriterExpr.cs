using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnStmt;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnLanguage
{
    
    /// <summary>
    /// 处理赋值表达式
    /// </summary>
    public MigxnVariable? VisitWriter(ExpressionContext context)
    {
        if (context is SingleExprContext single)
        {
            Result<MigxnVariable>? result = single.value.Type switch
            {
                MigxnLiteral.Name => Scopes.LoadVariable(single.value.Text),
                MigxnLiteral.RawName => Scopes.LoadVariable(single.value.Text[1..]),
                _ => default(Result<MigxnVariable>?)
            };
            if (result.HasValue)
            {
                if (result.Value.IsSuccess) return result.Value;
                Exceptions.Add(MigxnDiagnostic.Create(single.value, result.Value.Exception!));
                return null;
            }
        }

        return null;
    }
    
}