using MiHoMiao.Migxn.Syntax;

namespace MiHoMiao.Migxn.CodeAnalysis.Parser;

internal class ErrorTypeAssign(MigxnNode node, Type exprType, Type varType) : Exception
{
    public override string Message => $"ErrorTypeAssign at line {node.Position.Line} column {node.NextColumn}: " +
                                      $"Can not assign {exprType.Name} value to {varType.Name} variable.";
}