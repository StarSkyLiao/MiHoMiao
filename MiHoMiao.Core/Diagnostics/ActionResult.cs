using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Core.Diagnostics;

public readonly record struct ActionResult<T> : IResult<T> where T : class
{
    public object Value { get; }
    
    public bool IsSuccess { get; init; }
    
    public static implicit operator ActionResult<T>(T value) => new ActionResult<T>(value);
    
    public static implicit operator ActionResult<T>(Exception value) => new ActionResult<T>(value);
    
    public static implicit operator T?(ActionResult<T> result) => result.Value as T;
    
    public ActionResult(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    public ActionResult(Exception exception)
    {
        Value = exception;
        IsSuccess = false;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = Value as T;
        return IsSuccess;
    }
    
}