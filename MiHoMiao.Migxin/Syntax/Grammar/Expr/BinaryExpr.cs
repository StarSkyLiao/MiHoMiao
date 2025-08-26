using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxin.Syntax.Lexical;

namespace MiHoMiao.Migxin.Syntax.Grammar.Expr;

internal record BinaryExpr(MigxinExpr Left, IBinarySymbol BinarySymbol, MigxinExpr Right)
    : MigxinExpr($"({Left.Text}{BinarySymbol.Text}{Right.Text})".AsMemory(), Left.Index, Left.Position)
{
    public IBinarySymbol BinarySymbol { get; init; } = BinarySymbol;
    internal override IEnumerable<MigxinTree> Children() => [Left, Right];
    
    internal static MigxinExpr CombineBinary(MigxinExpr left, IBinarySymbol binarySymbol, MigxinExpr right)
    {
        switch (right)
        {
            case BinaryExpr nextBinary when nextBinary.BinarySymbol.Priority >= binarySymbol.Priority:
            {
                MigxinExpr binaryExpr = CombineBinary(left, binarySymbol, nextBinary.Left);
                return new BinaryExpr(binaryExpr, nextBinary.BinarySymbol, nextBinary.Right);
            }
            // case SuffixExpr nextSuffix when nextSuffix.SuffixToken.Priority >= binary.Priority:
            // {
            //     BinaryExpr binaryExpr = new BinaryExpr(binary, binary, nextSuffix.Left);
            //     return new SuffixExpr(binaryExpr, nextSuffix.SuffixToken);
            // }
            // case FuncCallExpr funcCallExpr when binary.Priority == 0:
            // {
            //     MigxinExpr binaryExpr = CombineBinary(binary, binary, funcCallExpr.Method);
            //     return new FuncCallExpr(binaryExpr, funcCallExpr.Left, funcCallExpr.ParamList, funcCallExpr.Right);
            // }
        }
        return new BinaryExpr(left, binarySymbol, right);
    }
    
    [field: AllowNull, MaybeNull]
    private static List<Type> BinaryTypes => field ??=
    [
        ..
        from type in typeof(MigxinToken).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(IBinarySymbol)) && !type.IsAbstract
        select type
    ];    
    
    [field: AllowNull, MaybeNull]
    internal static Dictionary<Type, IBinarySymbol.BinaryParser> BinaryParsers
    {
        get
        {
            if (field != null) return field;

            field = [];
            foreach (Type type in BinaryTypes)
            {
                object? tokenType = type.GetProperty(nameof(IBinarySymbol.TokenType))?.GetValue(null);
                Debug.Assert(tokenType != null);
                var func = type.GetMethod(nameof(IBinarySymbol.TryMatch))?.CreateDelegate<IBinarySymbol.BinaryParser>();
                Debug.Assert(func != null);
                field.Add((Type)tokenType, func);
            }

            return field;
        }
    }

}