using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Diagnostics.Benchmarks;

public record MethodWrapper<TResult>(Func<TResult> Method, [CallerArgumentExpression(nameof(Method))] string MethodName = null!)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal TResult Execute() => Method.Invoke();

    public static implicit operator MethodWrapper<TResult>(Func<TResult> other) => new MethodWrapper<TResult>(other);
    
    internal BenchmarkResult<TResult>.MathInfo CollectData(BenchmarkOption option, Func<Func<TResult>, BenchmarkOption, uint, decimal[]> measure)
    {
        if (option.LoopCount < 1) option = option with { LoopCount = 1 };
        uint iterations = option.Iterations;
        decimal[] eachTest = measure(Method, option, iterations);
        if ((option.Option & BenchmarkFlags.FullTest) == 0)
        {
            uint takeCount = Math.Max(iterations - (iterations >> 2), 1);
            eachTest.Sort();
            eachTest = eachTest[..(int)takeCount];
        }

        decimal summary = eachTest.Select(item => item / option.LoopCount).Sum();
        decimal mean = summary / eachTest.Length;
        decimal variance = Statistics.VariancePopulation(eachTest.Select(item => item / option.LoopCount).ToArray());

        return new BenchmarkResult<TResult>.MathInfo(summary, mean, variance, Method());
    }
    
}