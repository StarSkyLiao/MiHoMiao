using MiHoMiao.Migxn.CodeGen.Compare;
using MiHoMiao.Migxn.CodeGen.Data.Load;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitNotEqual(Type leftType, Type rightType, int leftTail)
    {
        Type result = EmitEql(leftType, rightType, leftTail);
        if (result == typeof(void)) return result;
        MigxnContext.EmitCode(new OpLdcLong(0));
        MigxnContext.EmitCode(new OpCeq());
        return result;
    }
}