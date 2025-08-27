using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Grammar;

namespace MiHoMiao.Migxin.Compiler.Syntax.Expr.Binary;

internal interface IOperatorSymbol
{
    /// <summary>
    /// 优先级越小, 实际优先级越高
    /// </summary>
    public abstract uint Priority { get; }
    
    /// <summary>
    /// 该符号对应的文本
    /// </summary>
    public abstract string Text { get; }
    
    /// <summary>
    /// 对应的原始 Token 的类型
    /// </summary>
    public static abstract Type TokenType { get; }

    /// <summary>
    /// 生成指定的 Symbol.
    /// </summary>
    public static abstract IOperatorSymbol LoadSymbol();

    public delegate MigxinResult<MigxinExpr> BinaryParser(MigxinExpr? left, MigxinGrammar migxinGrammar);

    public static abstract MigxinResult<MigxinExpr> TryMatch(MigxinExpr? left, MigxinGrammar migxinGrammar);
    
    protected static MigxinResult<MigxinExpr> TryMatchSymbol<T>(MigxinExpr? left, MigxinGrammar migxinGrammar)
        where T : IOperatorSymbol
    {
        if (left == null) return new DiagnosticBag(new ShouldBe(migxinGrammar.Position, nameof(MigxinExpr)));
        MigxinResult<MigxinExpr> right = MigxinExpr.TryParse(migxinGrammar);
        if (right.IsSuccess) return BinaryExpr.CombineBinary(left, T.LoadSymbol(), right.Result!);
        right.Exception!.Attach(new ShouldBe(migxinGrammar.Position, nameof(MigxinExpr)));
        return right;
    }
    
}