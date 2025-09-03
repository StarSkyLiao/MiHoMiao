using MiHoMiao.Migxn.CodeGen.Algorithm;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitDiv(Type leftType, Type rightType, int leftTail)
        => EmitBinary<OpDiv>(leftType, rightType, leftTail, "op_Division");
}