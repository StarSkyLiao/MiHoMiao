using System.Diagnostics;
using MiHoMiao.Migxn.CodeGen.Flow;
using MiHoMiao.Migxn.Runtime.Members;
using static MiHoMiao.Migxn.Antlr.Generated.MigxnLanguage;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    public override Type? VisitReturnStmt(ReturnStmtContext context)
    {
        MigxnMethod? method = MigxnContext.MigxnMember as MigxnMethod;
        Debug.Assert(method != null);
        if (context.Result != null) Visit(context.Result);
        MigxnContext.EmitCode(new OpGoto(method.ReturnLabel));
        return null;
    }
    
}