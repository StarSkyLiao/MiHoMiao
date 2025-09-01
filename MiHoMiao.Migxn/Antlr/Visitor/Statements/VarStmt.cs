using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type? VisitVarStmt(VarStmtContext context)
    {
        Type? varType = Visit(context.Expression);
        
        string name = context.VarName.Text;
        Exception? exception = MigxnContext.MigxnScope.DeclareVariable(new LocalVariable(name, varType) { IsWritable = true });
        
        if (exception is null)
        {
            MigxnContext.EmitCode(new OpStVar(name));
        }
        else
        {
            MigxnContext.EmitCode(new OpError(exception.Message));
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.VarName, exception));
        }
        return null;
    }
    
}