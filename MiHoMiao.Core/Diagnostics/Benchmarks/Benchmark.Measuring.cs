using System.Diagnostics;

namespace MiHoMiao.Core.Diagnostics.Benchmarks;

public static partial class Benchmark
{
    private static decimal[] MeasureTime<TResult>(Func<TResult> method, BenchmarkOption option, uint iterations)
    {
        Stopwatch stopwatch = new Stopwatch();
        
        decimal[] eachTest = new decimal[iterations];
        for (int i = 0; i < iterations; i++)
        {
            Benchmark.GcCollect(BenchmarkFlags.None);
            TimeSpan old = stopwatch.Elapsed;
            stopwatch.Start();
            for (int index = (int)option.LoopCount; index > 0; index--) method.Invoke();
            stopwatch.Stop();
            eachTest[i] = (decimal)(stopwatch.Elapsed.Ticks - old.Ticks) / TimeSpan.TicksPerSecond;
            Benchmark.GcCollect(BenchmarkFlags.None);
        }

        return eachTest;
    }
    
    private static decimal[] MeasureMemory<TResult>(Func<TResult> method, BenchmarkOption option, uint iterations)
    {
        decimal[] eachTest = new decimal[iterations];
        for (int i = 0; i < iterations; i++)
        {
            Benchmark.GcCollect(BenchmarkFlags.None);
            long memoryStart = GC.GetTotalAllocatedBytes(true);
            for (int index = (int)option.LoopCount; index > 0; index--) method.Invoke();
            long memoryEnd = GC.GetTotalAllocatedBytes(true);
            eachTest[i] = memoryEnd - memoryStart;
            Benchmark.GcCollect(BenchmarkFlags.None);
        }

        return eachTest;
    }
}