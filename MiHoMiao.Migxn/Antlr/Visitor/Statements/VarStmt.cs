using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.CodeGen.Data.Store;
using MiHoMiao.Migxn.Runtime.Members;
using MiHoMiao.Migxn.Runtime.Variable;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnStmt;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnLanguage
{
    public override Type? VisitVarStmt(VarStmtContext context)
    {
        Type? varType = Visit(context.Expression);
        
        string name = context.VarName.Text;
        Exception? exception = Scopes.DeclareVariable(new LocalVariable(name, varType) { IsWritable = true });
        
        if (exception is null)
        {
            Codes.Add(new OpStVar(name));
        }
        else
        {
            Codes.Add(new OpError(exception.Message));
            Exceptions.Add(MigxnDiagnostic.Create(context.VarName, exception));
        }
        return null;
    }
    
}