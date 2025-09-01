using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Antlr.Visitor;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Antlr;

internal partial class MigxnMemberParser
{
    public override MigxnMember VisitMethod(MigxnLanguage.MethodContext context)
    {
        string name = context.FuncName.Text;
        MigxnMethod migxnMethod = new MigxnMethod(MigxnContext, name, typeof(void));
        MigxnContext.AllMembers.Add(migxnMethod);
        new MigxnMethodParser(MigxnContext).Visit(context.Body);
        migxnMethod.SetReturn();
        return null;
    }
    
}