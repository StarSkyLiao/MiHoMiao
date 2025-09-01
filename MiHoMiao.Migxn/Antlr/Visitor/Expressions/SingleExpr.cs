using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    
    /// <summary>
    /// 处理单值表达式（整数或浮点数）
    /// </summary>
    public override Type? VisitSingleExpr(SingleExprContext context)
    {
        string text = context.GetText();
        if (context.Value.Type is MigxnLiteral.Name)
        {
            string varName = context.Value.Text;
            Result<MigxnVariable> variable = MigxnContext.MigxnScope.LoadVariable(varName);
            if (!variable.IsSuccess)
            {
                MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Value, variable.Exception!));
                MigxnContext.EmitCode(new OpError(variable.Exception!.Message));
                return null;
            }

            MigxnContext.EmitCode(new OpLdVar(varName));
            return variable.Value.Type;
        }

        (Type? type, MigxnOpCode opCode) = context.Value.Type switch
        {
            MigxnLiteral.Integer => (typeof(long), new OpLdcLong(long.Parse(text))),
            MigxnLiteral.Float => (typeof(double), new OpLdcFloat(double.Parse(text))),
            _ => new ValueTuple<Type?, MigxnOpCode>(null, null!)
        };
        if (type == null) throw new UnreachableException();
        MigxnContext.EmitCode(opCode);
        return type;
        
    }
    
}