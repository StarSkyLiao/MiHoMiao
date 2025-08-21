using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Migxn.CodeAnalysis;

internal readonly record struct Diagnostic<T> : IResult<T> where T : class
{
    public object Value { get; }
    
    public bool IsSuccess { get; init; }
    
    public static implicit operator Diagnostic<T>(T value) => new Diagnostic<T>(value);
    
    public static implicit operator T?(Diagnostic<T> result) => result.Value as T;
    
    public Diagnostic(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    public Diagnostic(BadMigxnTree exception)
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