using MiHoMiao.Migxn.CodeGen.Compare;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitCle(Type leftType, Type rightType, int leftTail)
        => EmitCompare<OpCle>(leftType, rightType, leftTail, "op_LessThanOrEqual");
}