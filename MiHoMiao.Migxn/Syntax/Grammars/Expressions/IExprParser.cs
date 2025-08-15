namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public interface IExprParser<out TResult> where TResult : class
{
    public static abstract CodeAnalysis.IResult<TResult> TryParse(MigxnGrammar grammar);
}