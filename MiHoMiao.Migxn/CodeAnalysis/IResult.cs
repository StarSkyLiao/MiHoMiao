namespace MiHoMiao.Migxn.CodeAnalysis;

public interface IResult<out TResult> where TResult : class
{
    /// <summary>
    /// 包含的值
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// 返回结果是否正常
    /// </summary>
    public bool IsSuccess { get; init; }
    
    public BadMigxnTree? Exception => Value as BadMigxnTree;
    
    public TResult? Result => Value as TResult;
    
}