//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Diagnostics;
using System.Text;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.Core.Diagnostics;

/// <summary>
/// 用于测算一个方法的耗时
/// </summary>
public static class TimeTest
{
    private static readonly StringBuilder s_StringBuilder = new StringBuilder(4096);
    
    private static readonly Stopwatch s_Stopwatch = new Stopwatch();
    
    /// <summary>
    /// 运行时间使用测试，测量指定方法的时间消耗量。
    /// </summary>
    public static void RunTest(Action testAction, string? name = null, int iterations = 1, RunTestOption option = 0)
    {
        s_StringBuilder.Clear();
        s_StringBuilder.Append("------------------------------");
        s_StringBuilder.Append($"{name ?? testAction.ToString()}");
        s_StringBuilder.AppendLine("------------------------------");
        
        if ((option & RunTestOption.Warm) != 0) testAction();
        
        s_Stopwatch.Reset();
        
        Span<double> eachTicks = iterations > 512 ? new double[iterations] : stackalloc double[iterations];
        for (int i = 0; i < iterations; i++)
        {
            double old = s_Stopwatch.Elapsed.TotalSeconds;
            s_Stopwatch.Start();
            testAction();
            s_Stopwatch.Stop();
            eachTicks[i] = s_Stopwatch.Elapsed.TotalSeconds - old;
        }

        s_StringBuilder.AppendLine("Perf Result:");
        s_StringBuilder.AppendLine($"--{iterations} Times Costs: {s_Stopwatch.Elapsed.TotalSeconds.NumberString("G5")}s");
        s_StringBuilder.AppendLine($"--Each Cost: {(s_Stopwatch.Elapsed.TotalSeconds / iterations).NumberString("G5")}s");
        
        if ((option & RunTestOption.Best75) != 0)
        {
            int takeCount = (iterations - (iterations >> 2)).Min(1);
            eachTicks.Sort();
            eachTicks = eachTicks[..takeCount];
            double fast75Time = 0;
            foreach (double item in eachTicks) fast75Time += item;
            
            s_StringBuilder.AppendLine("As for the fastest 75%:");
            s_StringBuilder.AppendLine($"--(75%){takeCount} Times Costs: {fast75Time.NumberString("G5")}s");
            s_StringBuilder.AppendLine($"--(75%)Each Cost: {(fast75Time / takeCount).NumberString("G5")}s");
        }
        
        if ((option & RunTestOption.Sequence) != 0)
        {
            foreach (double item in eachTicks) s_StringBuilder.Append($"{item.NumberString("F1")}s ");
        }

        s_StringBuilder.AppendLine("-----------------------------------------------------------------");
        Console.WriteLine(s_StringBuilder.ToString());
    }
    
    [Flags]
    public enum RunTestOption: byte
    {
        Warm         = 0b0000_0001,
        Sequence     = 0b0000_0010,
        Best75       = 0b0000_0100,
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

}