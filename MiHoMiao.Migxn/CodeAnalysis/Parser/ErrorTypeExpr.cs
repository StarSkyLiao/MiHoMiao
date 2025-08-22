using MiHoMiao.Migxn.Syntax;

namespace MiHoMiao.Migxn.CodeAnalysis.Parser;

internal class ErrorTypeExpr(MigxnNode node, Type exprType, Type varType, string @operator) : Exception
{
    public override string Message => $"ErrorTypeExpr at line {node.Position.Line} column {node.NextColumn}: " +
                                      $"No {@operator} operator between {exprType.Name} and {varType.Name}.";
}