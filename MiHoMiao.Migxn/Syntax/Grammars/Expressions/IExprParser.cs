using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public interface IExprParser<out TResult> where TResult : class
{
    public static abstract IResult<TResult> TryParse(MigxnGrammar grammar);
}