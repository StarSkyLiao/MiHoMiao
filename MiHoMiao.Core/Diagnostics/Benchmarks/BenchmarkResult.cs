using System.Text;

namespace MiHoMiao.Core.Diagnostics.Benchmarks;

internal record BenchmarkResult<TResult>(MethodWrapper<TResult> Method, BenchmarkResult<TResult>.MathInfo Time, BenchmarkResult<TResult>.MathInfo Memory)
{
    public readonly record struct MathInfo(decimal Sum, decimal Mean, decimal Variance, TResult Return)
    {
        public string ToString(string header, string format) => new StringBuilder()
            .Append($"{header,4}")
            .Append($"{$"{Sum.Fmt("F2")}{format}",15}")
            .Append($"{$"{Mean.Fmt("F2")}{format}",15}")
            .Append($"{$"{Variance.Fmt("F2")}{format}",15}")
            .Append($"{((Mean is not 0) ? (1 - Variance / Mean) : 1),12:P2}")
            .Append($"      {Return,-12}")
            .ToString();
    }
}