using MiHoMiao.Core.Reflection;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Compiler;
using MiHoMiao.Migxin.Compiler.Semantic.Stmt;
using MiHoMiao.Migxin.Compiler.Syntax.Expr;
using MiHoMiao.Migxin.Compiler.Syntax.Expr.Prefix;
using MiHoMiao.Migxin.Compiler.Syntax.Expr.Suffix;

namespace MiHoMiao.Migxin.Compiler.Semantic.Expr;

internal class VisitMigxinExpr : ITreeVisitor<VisitMigxinExpr, MigxinExpr>
{
    public static void Visit(MigxinParser migxinParser, MigxinExpr tree)
    {
    }

    public static MigxinType EvaluateExprType(MigxinParser parser, MigxinExpr expr)
    {
        switch (expr)
        {
            case LiteralExpr literalExpr: return literalExpr.Token.LiteralType;
            case SuffixExpr suffixExpr:
                switch (suffixExpr.OperatorSymbol)
                {
                    case NotNullSymbol:
                        MigxinType migxinType = EvaluateExprType(parser, suffixExpr.Left);
                        if (migxinType.Nullable)
                            parser.Warnings.Add(
                                new StringWarning(suffixExpr.Position, "Unnecessary nullable suppressors.")
                            );
                        return migxinType with { Nullable = true };
                    default: return typeof(object);
                }
            case PrefixExpr prefix:
                
                return null;
        }
        
        return typeof(object);
    }
}