using MiHoMiao.Migxn.CodeGen.Algorithm;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitRem(Type leftType, Type rightType, int leftTail)
        => EmitBinary<OpRem>(leftType, rightType, leftTail, "op_Modulus");
}