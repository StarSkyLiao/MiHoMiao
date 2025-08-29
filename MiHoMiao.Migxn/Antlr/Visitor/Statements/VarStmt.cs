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
        MigxnScope scope = MigxnMethod.Context.MigxnScope;
        if (scope.IsAbleToDeclareVariable(name))
        {
            scope.DeclareVariable(new LocalVariable(name, varType));
            MigxnMethod.Codes.Add(new OpStVar(name));
        }
        else
        {
            
        }
        return null;
    }
    
}