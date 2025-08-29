using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnStmt;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnLanguage
{
    
    /// <summary>
    /// 处理单值表达式（整数或浮点数）
    /// </summary>
    public override Type? VisitSingleExpr(SingleExprContext context)
    {
        string text = context.GetText();
        if (context.value.Type is MigxnLiteral.Name)
        {
            string varName = context.value.Text;
            Result<MigxnVariable> variable = MigxnMethod.Context.MigxnScope.LoadVariable(varName);
            if (!variable.IsSuccess)
            {
                MigxnMethod.Context.Exceptions.Add(variable.Exception!);
                MigxnMethod.Codes.Add(new OpError($"Non-Exist var:{varName}"));
                return null;
            }

            MigxnMethod.Codes.Add(new OpLdVar(varName));
            return variable.Value.Type;
        }

        (Type? type, MigxnOpCode opCode) = context.value.Type switch
        {
            MigxnLiteral.Integer => (typeof(long), new OpLdcLong(long.Parse(text))),
            MigxnLiteral.Float => (typeof(double), new OpLdcFloat(double.Parse(text))),
            _ => new ValueTuple<Type?, MigxnOpCode>(null, null!)
        };
        if (type == null) throw new UnreachableException();
        MigxnMethod.Codes.Add(opCode);
        return type;
        
    }
    
}