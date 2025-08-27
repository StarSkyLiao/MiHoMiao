using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxin.Syntax.Grammar.Expr.Prefix;
using MiHoMiao.Migxin.Syntax.Grammar.Expr.Suffix;
using MiHoMiao.Migxin.Syntax.Lexical;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr.Binary;

internal record BinaryExpr(MigxinExpr Left, IOperatorSymbol OperatorSymbol, MigxinExpr Right)
    : MigxinExpr($"({Left.Text}{OperatorSymbol.Text}{Right.Text})".AsMemory(), Left.Index, Left.Position), ISymbolConnected
{
    internal override IEnumerable<MigxinTree> Children() => [Left, Right];
    
    internal static MigxinExpr CombineBinary(MigxinExpr? left, IOperatorSymbol operatorSymbol, MigxinExpr right)
    {
        switch (right)
        {
            case BinaryExpr nextBinary when nextBinary.OperatorSymbol.Priority >= operatorSymbol.Priority:
            {
                MigxinExpr binaryExpr = CombineBinary(left, operatorSymbol, nextBinary.Left);
                return new BinaryExpr(binaryExpr, nextBinary.OperatorSymbol, nextBinary.Right);
            }
            case SuffixExpr nextSuffix when nextSuffix.OperatorSymbol.Priority >= operatorSymbol.Priority:
            {
                MigxinExpr front = (left == null)
                    ? new PrefixExpr(operatorSymbol, nextSuffix.Left)
                    : new BinaryExpr(left, operatorSymbol, nextSuffix.Left);
                return new SuffixExpr(front, nextSuffix.OperatorSymbol);

            }
            // case FuncCallExpr funcCallExpr when binary.Priority == 0:
            // {
            //     MigxinExpr binaryExpr = CombineBinary(binary, binary, funcCallExpr.Method);
            //     return new FuncCallExpr(binaryExpr, funcCallExpr.Left, funcCallExpr.ParamList, funcCallExpr.Right);
            // }
        }

        return left is null ? new PrefixExpr(operatorSymbol, right) : new BinaryExpr(left, operatorSymbol, right);
    }

    #region Reflection

    [field: AllowNull, MaybeNull]
    private static List<Type> BinaryTypes => field ??=
    [
        ..
        from type in typeof(MigxinToken).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(IOperatorSymbol)) && !type.IsAbstract && !type.IsInterface
        select type
    ];    
    
    [field: AllowNull, MaybeNull]
    internal static Dictionary<Type, IOperatorSymbol.BinaryParser> BinaryParsers
    {
        get
        {
            if (field != null) return field;

            field = [];
            foreach (Type type in BinaryTypes)
            {
                object? tokenType = type.GetProperty(nameof(IOperatorSymbol.TokenType))?.GetValue(null);
                Debug.Assert(tokenType != null);
                var func = type.GetMethod(nameof(IOperatorSymbol.TryMatch))?.CreateDelegate<IOperatorSymbol.BinaryParser>();
                Debug.Assert(func != null);
                field.Add((Type)tokenType, func);
            }

            return field;
        }
    }

    #endregion

}