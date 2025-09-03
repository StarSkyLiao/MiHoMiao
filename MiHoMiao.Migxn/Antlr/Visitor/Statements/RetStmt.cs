using System.Diagnostics;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Flow;
using MiHoMiao.Migxn.Runtime.Members;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type VisitReturnStmt(ReturnStmtContext context)
    {
        MigxnMethod? method = MigxnContext.MigxnMember as MigxnMethod;
        Debug.Assert(method != null);

        Type result = typeof(void);
        if (context.Result != null) result = Visit(context.Result);
        
        MigxnContext.EmitCode(new OpCast(result, method.ReturnType, true));
        MigxnContext.EmitCode(new OpGoto(method.ReturnLabel));
        return typeof(void);
    }
    
}