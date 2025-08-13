namespace MiHoMiao.Core.Diagnostics;

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
    
    public Exception? Exception => Value as Exception;
    
    public TResult? Result => Value as TResult;
    
}