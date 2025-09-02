using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal sealed partial class MigxnMethodParser(MigxnContext context) : MigxnLanguageBaseVisitor<Type>
{
    public readonly MigxnContext MigxnContext = context;
    // public readonly MigxnMethod MigxnMethod = method;
    //
    // public readonly List<MigxnDiagnostic> Exceptions = method.Context.Exceptions;
    //
    // public readonly List<MigxnOpCode> Codes = method.Codes;
    //
    // public readonly MigxnScope Scopes = method.Context.MigxnScope;
    
}