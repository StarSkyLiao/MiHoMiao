using MiHoMiao.Migxn.CodeAnalysis;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

internal interface IExprParser<out TResult> where TResult : class
{
    public static abstract IResult<TResult> TryParse(MigxnGrammar grammar);
}