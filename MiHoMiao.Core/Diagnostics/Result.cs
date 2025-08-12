//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Core.Diagnostics;

public readonly struct Result<T>
{
    /// <summary>
    /// 包含的值
    /// </summary>
    [field: AllowNull]
    public T Value { get; }

    /// <summary>
    /// 包含的异常
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// 返回结果是否正常
    /// </summary>
    public bool IsSuccess => Exception == null;
    
    public static implicit operator Result<T>(T value) => new Result<T>(value);
    
    public static implicit operator Result<T>(Exception value) => new Result<T>(value);
    
    public static implicit operator T(Result<T> result) => result.Value;
    
    public Result(T value) => Value = value;
    
    public Result(Exception? exception) => Exception = exception;
    
    public Result<TOther> ConvertTo<TOther>()
    {
        if (!IsSuccess) return new Result<TOther>(Exception);
        if (Value is TOther compatibleValue) return new Result<TOther>(compatibleValue);
        throw new InvalidCastException(
            $"Cannot convert {(Value == null ? null : Value!.GetType())} to {typeof(TOther)}"
        );
    }

}