using MiHoMiao.Migxn.CodeGen.Algorithm;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitMul(Type leftType, Type rightType, int leftTail)
        => EmitBinary<OpMul>(leftType, rightType, leftTail, "op_Multiply");
}