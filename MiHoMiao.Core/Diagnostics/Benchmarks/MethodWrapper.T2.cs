using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Diagnostics.Benchmarks;

public sealed record MethodWrapper<T1, TResult>(Func<T1, TResult> InitMethod, T1 Param1,
    [CallerArgumentExpression(nameof(InitMethod))] string? MethodName = null
) : MethodWrapper<TResult>(()=> InitMethod(Param1), MethodName);

public sealed record MethodWrapper<T1, T2, TResult>(Func<T1, T2, TResult> InitMethod, T1 Param1, T2 Param2, 
    [CallerArgumentExpression(nameof(InitMethod))] string? MethodName = null
) : MethodWrapper<TResult>(()=> InitMethod(Param1, Param2), MethodName);