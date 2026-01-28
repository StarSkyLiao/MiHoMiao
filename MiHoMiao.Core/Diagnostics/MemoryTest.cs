using System.Text;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.Core.Diagnostics;

/// <summary>
/// 用于测算一个方法的内存分配量
/// </summary>
public static class MemoryTest
{
    private static readonly StringBuilder s_StringBuilder = new StringBuilder(4096);
    
    /// <summary>
    /// 运行时间使用测试，测量指定方法的时间消耗量。
    /// </summary>
    public static void RunTest(Action testAction, string? name = null, int iterations = 1, PerfTestOption option = 0)
    {
        s_StringBuilder.Clear();
        s_StringBuilder.Append("------------------------------");
        s_StringBuilder.Append($"{name ?? testAction.ToString()}");
        s_StringBuilder.AppendLine("------------------------------");
        
        // 强制 GC 清理以确保初始内存状态
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // 预热运行
        if ((option & PerfTestOption.Warm) != 0) testAction();
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        double totalMemoryBytes = 0;
        Span<double> eachMemory = iterations > 512 ? new double[iterations] : stackalloc double[iterations];
        for (int i = 0; i < iterations; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            long memoryStart = GC.GetTotalAllocatedBytes(true);
            testAction();
            long memoryEnd = GC.GetTotalAllocatedBytes(true);
            eachMemory[i] = memoryEnd - memoryStart;
            totalMemoryBytes += memoryEnd - memoryStart;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        s_StringBuilder.AppendLine("Memory Result:");
        s_StringBuilder.AppendLine($"--{iterations} Times Costs: {totalMemoryBytes.NumberString("G5")} Bytes");
        s_StringBuilder.AppendLine($"--Each Cost: {(totalMemoryBytes / iterations).NumberString("G5")} Bytes");
        
        if ((option & PerfTestOption.Best75) != 0)
        {
            int takeCount = (iterations - (iterations >> 2)).Min(1);
            eachMemory.Sort();
            eachMemory = eachMemory[..takeCount];
            double fast75Time = 0;
            foreach (double item in eachMemory) fast75Time += item;
            
            s_StringBuilder.AppendLine("As for the best 75%:");
            s_StringBuilder.AppendLine($"--(75%){takeCount} Times Costs: {fast75Time.NumberString("G5")} Bytes");
            s_StringBuilder.AppendLine($"--(75%)Each Cost: {(fast75Time / takeCount).NumberString("G5")} Bytes");
            double warmup = totalMemoryBytes - fast75Time / takeCount * iterations;
            s_StringBuilder.AppendLine($"Warmup Cost: {warmup.NumberString("G5")} Bytes");
        }
        
        if ((option & PerfTestOption.Sequence) != 0)
        {
            s_StringBuilder.AppendLine("Sequence Result:");
            foreach (double item in eachMemory) s_StringBuilder.Append($"{item.NumberString("F1")} Bytes ");
            s_StringBuilder.AppendLine();
        }

        s_StringBuilder.AppendLine("-----------------------------------------------------------------");
        Console.WriteLine(s_StringBuilder.ToString());
    }
    
    private static string NumberString(this double value, string? format = null) => value switch
    {
        >= 1e+15 => $"{(value / 1e+15).ToString(format)}T",
        >= (1 << 30) and < (1L << 40) => $"{(value / 1e+9).ToString(format)}G",
        >= (1 << 20) and < (1 << 30) => $"{(value / 1e+6).ToString(format)}M",
        >= (1 << 10) and < (1 << 20) => $"{(value / 1e+3).ToString(format)}K",
        >= 1 and < (1 << 10) => $"{(value / 1).ToString(format)}",
        _ => value.ToString(format)
    };
    
}