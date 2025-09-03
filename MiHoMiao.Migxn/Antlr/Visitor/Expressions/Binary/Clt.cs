using MiHoMiao.Migxn.CodeGen.Compare;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitClt(Type leftType, Type rightType, int leftTail)
        => EmitCompare<OpClt>(leftType, rightType, leftTail, "op_LessThan");
}