using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Antlr;

internal partial class MigxnMemberParser(MigxnContext context) : MigxnLanguageBaseVisitor<MigxnMember?>
{
    public MigxnContext MigxnContext { get; } = context;
    
}