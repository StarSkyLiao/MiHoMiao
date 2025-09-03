using MiHoMiao.Migxn.CodeGen.Algorithm;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitSub(Type leftType, Type rightType, int leftTail)
        => EmitBinary<OpSub>(leftType, rightType, leftTail, "op_Subtraction");
}