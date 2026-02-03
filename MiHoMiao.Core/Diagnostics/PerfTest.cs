using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Diagnostics;

public static class PerfTest
{
    public static TextWriter Output { get; set; } = Console.Out;

    public static void RunTest<TResult>(Func<TResult> testAction,
        PerfTestSetting option, [CallerArgumentExpression(nameof(testAction))] string? name = null
    ) => RunTestInternal(testAction, option, name?.Replace('\n', ' ').Replace('\r', ' ') ?? "null");
    
    public static void RunTest<TResult, T1>(Func<T1, TResult> testAction,
        T1 arg1,
        PerfTestSetting option, [CallerArgumentExpression(nameof(testAction))] string? name = null
    ) => RunTestInternal(() => testAction(arg1), option, name?.Replace('\n', ' ').Replace('\r', ' ') ?? "null");

    public static void RunTest<TResult, T1, T2>(Func<T1, T2, TResult> testAction,
        T1 arg1, T2 arg2,
        PerfTestSetting option, [CallerArgumentExpression(nameof(testAction))] string? name = null
    ) => RunTestInternal(() => testAction(arg1, arg2), option, name?.Replace('\n', ' ').Replace('\r', ' ') ?? "null");

    public static void RunTest<TResult, T1, T2, T3>(Func<T1, T2, T3, TResult> testAction,
        T1 arg1, T2 arg2, T3 arg3,
        PerfTestSetting option, [CallerArgumentExpression(nameof(testAction))] string? name = null
    ) => RunTestInternal(() => testAction(arg1, arg2, arg3), option, name?.Replace('\n', ' ').Replace('\r', ' ') ?? "null");

    private static void RunTestInternal<TResult>(Func<TResult> testAction, PerfTestSetting option, string testMethodName)
    {
        ConsoleColor defaultColor = Console.ForegroundColor; 
        Console.ForegroundColor = ConsoleColor.Cyan;
        Output.WriteLine(new string('-', 50));
        Console.ForegroundColor = ConsoleColor.Green;
        // 强制 GC 清理
        GcCollect(option.Option);
        // 预热运行
        if ((option.Option & PerfTestOption.Warm) != 0)
        {
            testAction();
            GcCollect(option.Option);
        }
        Output.WriteLine($"Method: {testMethodName}, Iterations: {option.Iterations}, TestOption: {option}");
        Output.Write($"{"MeasureType",12}");
        Output.Write($"{"Summary",12} ");
        Output.Write($"{"Average",12} ");
        Output.Write($"{"Error",12} ");
        Output.Write($"{"可信度",9}");
        Output.Write($"{' ',6}");
        Output.WriteLine("方法返回值");
        RunMemory(testAction, option);
        RunTime(testAction, option);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Output.WriteLine(new string('-', 50));
        Console.ForegroundColor = defaultColor;
    }

    public static void RunMemory<TResult>(Func<TResult> testAction, PerfTestSetting option)
    {
        if (option.LoopCount < 1) option = option with { LoopCount = 1 };
        uint iterations = option.Iterations;
        Span<double> eachTest = iterations > 512 ? new double[iterations] : stackalloc double[(int)iterations];
        for (int i = 0; i < iterations; i++)
        {
            GcCollect(PerfTestOption.None);
            long memoryStart = GC.GetTotalAllocatedBytes(true);
            for (int index = (int)option.LoopCount; index > 0; index--) testAction();
            long memoryEnd = GC.GetTotalAllocatedBytes(true);
            eachTest[i] = memoryEnd - memoryStart;
            GcCollect(PerfTestOption.None);
        }
        if ((option.Option & PerfTestOption.Best75) != 0)
        {
            uint takeCount = Math.Max(iterations - (iterations >> 2), 1);
            eachTest.Sort();
            eachTest = eachTest[..(int)takeCount];
        }
        
        double variance = Math.Sqrt(eachTest.VariancePopulation()) / option.LoopCount;
        double summary = eachTest.Summation() / option.LoopCount;
        double average = summary / iterations;

        Output.Write($"{"Memory",12}");
        Output.Write($"{summary.NumberString("F2"),12}B");
        Output.Write($"{average.NumberString("F2"),12}B");
        Output.Write($"{variance.NumberString("F2"),12}B");
        Output.Write($"{((average is not 0) ? (1 - variance / average) : 1),12:P2}");
        Output.Write($"{' ',6}");
        Output.Write($"{testAction()?.ToString()}");
        Output.WriteLine();
    }
    
    public static void RunTime<TResult>(Func<TResult> testAction, PerfTestSetting option)
    {
        Stopwatch stopwatch = new Stopwatch();
        if (option.LoopCount < 1) option = option with { LoopCount = 1 };
        uint iterations = option.Iterations;
        Span<double> eachTest = iterations > 512 ? new double[iterations] : stackalloc double[(int)iterations];
        for (int i = 0; i < iterations; i++)
        {
            GcCollect(PerfTestOption.None);
            TimeSpan old = stopwatch.Elapsed;
            stopwatch.Start();
            for (int index = (int)option.LoopCount; index > 0; index--) testAction();
            stopwatch.Stop();
            eachTest[i] = stopwatch.Elapsed.Ticks - old.Ticks;
            GcCollect(PerfTestOption.None);
        }
        if ((option.Option & PerfTestOption.Best75) != 0)
        {
            uint takeCount = Math.Max(iterations - (iterations >> 2), 1);
            eachTest.Sort();
            eachTest = eachTest[..(int)takeCount];
        }
        
        double variance = Math.Sqrt(eachTest.VariancePopulation()) / option.LoopCount / TimeSpan.TicksPerSecond;
        double summary = (double)stopwatch.Elapsed.Ticks / option.LoopCount / TimeSpan.TicksPerSecond;
        double average = summary / iterations;

        Output.Write($"{"Time",12}");
        Output.Write($"{summary.NumberString("F2"),12}s");
        Output.Write($"{average.NumberString("F2"),12}s");
        Output.Write($"{variance.NumberString("F2"),12}s");
        Output.Write($"{((average is not 0) ? (1 - variance / average) : 1),12:P2}");
        Output.Write($"{' ',6}");
        Output.Write($"{testAction()?.ToString()}");
        Output.WriteLine();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GcCollect(PerfTestOption option)
    {
        if ((option & PerfTestOption.NoGc) > 0) return;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    
    private static string NumberString(this double value, string? format = null) => value switch
    {
        >= 1e+15 => $"{(value / 1e+15).ToString(format)}T",
        >= 1e+12 and < 1e+15 => $"{(value / 1e+12).ToString(format)}T",
        >= 1e+9 and < 1e+12 => $"{(value / 1e+9).ToString(format)}G",
        >= 1e+6 and < 1e+9 => $"{(value / 1e+6).ToString(format)}M",
        >= 1e+3 and < 1e+6 => $"{(value / 1e+3).ToString(format)}K",
        >= 1e+0 and < 1e+3 => $"{(value / 1).ToString(format)}",
        >= 1e-3 and < 1e+0 => $"{(value / 1e-3).ToString(format)}m",
        >= 1e-6 and < 1e-3 => $"{(value / 1e-6).ToString(format)}μ",
        >= 1e-9 and < 1e-6 => $"{(value / 1e-9).ToString(format)}n",
        < 1e-9 => $"{(value / 1e-12).ToString(format)}n",
        _ => value.ToString(format)
    };
    
    extension(ReadOnlySpan<double> span)
    {
        /// <summary>
        /// 计算总体方差 (Population Variance) - 除以 N
        /// 适用于完整数据集
        /// </summary>
        private double VariancePopulation()
        {
            double mean = 0;
            double m2 = 0;
        
            for (int i = 0; i < span.Length; i++)
            {
                double x = span[i];
                double delta = x - mean;
                mean += delta / (i + 1);
                double delta2 = x - mean;
                m2 += delta * delta2;
            }

            return m2 / span.Length;
        }

        /// <summary>
        /// 计算数据总和
        /// </summary>
        private double Summation()
        {
            double sum = 0;
            foreach (double item in span) sum += item;
            return sum;
        }
    }
    
    extension(ReadOnlySpan<TimeSpan> span)
    {
        private double VariancePopulation()
        {
            double mean = 0;
            double m2 = 0;
        
            for (int i = 0; i < span.Length; i++)
            {
                double x = span[i].Ticks;
                double delta = x - mean;
                mean += delta / (i + 1);
                double delta2 = x - mean;
                m2 += delta * delta2;
            }

            return m2 / span.Length;
        }

        /// <summary>
        /// 计算数据总和
        /// </summary>
        private double Summation()
        {
            double sum = 0;
            foreach (TimeSpan item in span) sum += item.Ticks;
            return sum;
        }
    }
}