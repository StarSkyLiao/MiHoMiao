using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis;

namespace MiHoMiao.Migxin.Compiler.Syntax;

internal class MigxinResult<TResult> where TResult : class
{
    /// <summary>
    /// 包含的值
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// 返回结果是否正常
    /// </summary>
    public bool IsSuccess { get; init; }
    
    public DiagnosticBag? Exception => Value as DiagnosticBag;
    
    public TResult? Result => Value as TResult;
    
    public static implicit operator MigxinResult<TResult>(DiagnosticBag exception) => new MigxinResult<TResult>(exception);
    
    public static implicit operator MigxinResult<TResult>(TResult value) => new MigxinResult<TResult>(value);
    
    public static implicit operator TResult?(MigxinResult<TResult> result) => result.Value as TResult;
    
    public MigxinResult<TOther> As<TOther>() where TOther : class => IsSuccess ? (Result as TOther)! : Exception!;

    public MigxinResult(TResult value)
    {
        Value = value;
        IsSuccess = true;
    }

    public MigxinResult(DiagnosticBag exception)
    {
        Value = exception;
        IsSuccess = false;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out TResult value)
    {
        value = Value as TResult;
        return IsSuccess;
    }
    
}
