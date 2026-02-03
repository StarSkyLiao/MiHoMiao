namespace MiHoMiao.Core.Diagnostics.Benchmarks;

public record struct BenchmarkOption(uint Iterations, BenchmarkFlags Option =  BenchmarkFlags.None)
{
    public uint LoopCount { get; init; } = 1;
}