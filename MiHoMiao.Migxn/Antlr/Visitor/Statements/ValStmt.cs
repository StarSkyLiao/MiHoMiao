using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type VisitValStmt(ValStmtContext context)
    {
        string name = context.VarName.Text;
        Type exprType = Visit(context.Expression);
        Type varType = context.Type != null ? TypeLoader.LoadType(context.Type.Text) : exprType;

        if (varType == typeof(void))
        {
            string message = $"Type of right variable \"{name}\" is void! This is not allowed!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Assign().Symbol, message));
            return typeof(void);
        }
        
        if (varType != exprType) MigxnContext.EmitCode(new OpCast(exprType, varType));
        
        Exception? exception = MigxnContext.MigxnScope.DeclareVariable(new LocalVariable(name, varType) { IsWritable = false });
        
        if (exception is null)
        {
            MigxnContext.EmitCode(new OpStVar(name));
        }
        else
        {
            MigxnContext.EmitCode(new OpError(exception.Message));
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.VarName, exception));
        }
        return typeof(void);
    }
    
}