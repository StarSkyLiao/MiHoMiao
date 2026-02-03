using System.Text;

namespace MiHoMiao.Core.Diagnostics.Benchmarks;

public static partial class Benchmark
{
    public static void RunTest<TResult>(BenchmarkOption option, params ReadOnlySpan<MethodWrapper<TResult>> methodList)
        => RunTestInternal(option, methodList);
    
    private static void RunTestInternal<TResult>(BenchmarkOption option, ReadOnlySpan<MethodWrapper<TResult>> methodList)
    {
        // 强制 GC 清理
        GcCollect(option.Option);
        // 预热运行
        if ((option.Option & BenchmarkFlags.NoWarm) == 0)
        {
            foreach (MethodWrapper<TResult> item in methodList) item.Execute();
            GcCollect(option.Option);
        }
        

        BenchmarkResult<TResult>[] resultList = new BenchmarkResult<TResult>[methodList.Length];
        for (int index = 0; index < methodList.Length; index++)
        {
            MethodWrapper<TResult> item = methodList[index];
            BenchmarkResult<TResult>.MathInfo timeInfo = item.CollectData(option, MeasureTime);
            BenchmarkResult<TResult>.MathInfo memoryInfo = item.CollectData(option, MeasureMemory);
            resultList[index] = new BenchmarkResult<TResult>(item, timeInfo, memoryInfo);
        }
        
        int maxLength = resultList.Max(item => item.Method.MethodName.Length);
        ColoredOut(ConsoleColor.Cyan, () =>
        {
            Console.WriteLine(new StringBuilder()
                .Append("方法名".PadRight(maxLength - 1))
                .Append("类型")
                .Append($"{"总和",13}")
                .Append($"{"平均",13}")
                .Append($"{"误差",13}")
                .Append($"{"可信度",9}")
                .Append($"      {"返回值",-9}")
                .ToString()
            );
        });
        for (int index = 0; index < resultList.Length; index++)
        {
            BenchmarkResult<TResult> benchmarkResult = resultList[index];
            ColoredOut((ConsoleColor)(index % 15 + 1), () =>
            {
                Console.Write(benchmarkResult.Method.MethodName.PadRight(maxLength));
                Console.WriteLine(benchmarkResult.Time.ToString("耗时", "s"));
            });
        }

        for (int index = 0; index < resultList.Length; index++)
        {
            BenchmarkResult<TResult> benchmarkResult = resultList[index];
            ColoredOut((ConsoleColor)(index % 15 + 1), () =>
            {
                Console.Write(benchmarkResult.Method.MethodName.PadRight(maxLength));
                Console.WriteLine(benchmarkResult.Memory.ToString("内存", "B"));
            });
        }
    }

    private static void ColoredOut(ConsoleColor color, Action action)
    {
        ConsoleColor defaultColor = Console.ForegroundColor; 
        Console.ForegroundColor = color;
        action();
        Console.ForegroundColor = defaultColor;
    }
    
    internal static void GcCollect(BenchmarkFlags option)
    {
        if ((option & BenchmarkFlags.NoExplicitGc) != 0) return;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    
}