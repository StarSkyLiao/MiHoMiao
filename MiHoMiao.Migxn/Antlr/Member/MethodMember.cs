using MiHoMiao.Migxn.Antlr.Generated;
using MiHoMiao.Migxn.Antlr.Visitor;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.Runtime.Members;

namespace MiHoMiao.Migxn.Antlr;

internal partial class MigxnMemberParser
{
    public override MigxnMember VisitMethod(MigxnLanguage.MethodContext context)
    {
        string name = context.FuncName.Text;
        string? typeString = context.ReturnType?.Text;
        Type? returnType = typeString != null ? TypeLoader.LoadType(typeString) : null;
        if (returnType == null)
        {
            string message = $"Undefined type \"{typeString}\"! This is not allowed!";
            MigxnContext.Exceptions.Add(MigxnDiagnostic.Create(context.Start, message));
        }
        MigxnMethod migxnMethod = new MigxnMethod(MigxnContext, name, returnType ?? typeof(object));
        MigxnContext.AllMembers.Add(migxnMethod);
        new MigxnMethodParser(MigxnContext).Visit(context.Body);
        migxnMethod.SetReturn();
        return migxnMethod;
    }
    
}