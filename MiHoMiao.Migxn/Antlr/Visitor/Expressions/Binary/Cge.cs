using MiHoMiao.Migxn.CodeGen.Compare;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitCge(Type leftType, Type rightType, int leftTail)
        => EmitCompare<OpCge>(leftType, rightType, leftTail, "op_GreaterThanOrEqual");
}