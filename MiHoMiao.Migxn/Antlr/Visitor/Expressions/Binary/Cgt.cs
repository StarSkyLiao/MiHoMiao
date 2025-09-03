using MiHoMiao.Migxn.CodeGen.Compare;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitCgt(Type leftType, Type rightType, int leftTail)
        => EmitCompare<OpCgt>(leftType, rightType, leftTail, "op_GreaterThan");
}