using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal sealed partial class MigxnLanguage(MigxnMethod method) : MigxnStmtBaseVisitor<Type?>
{
    public readonly MigxnMethod MigxnMethod = method;

    public readonly List<MigxnDiagnostic> Exceptions = method.Context.Exceptions;

    public readonly List<MigxnOpCode> Codes = method.Codes;
    
}