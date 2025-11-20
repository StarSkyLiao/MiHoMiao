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
        s_StringBuilder.Append("------------------------------");
        s_StringBuilder.Append($"{name ?? testAction.ToString()}");
        s_StringBuilder.AppendLine("------------------------------");
        
        if ((option & RunTestOption.Warm) != 0) testAction();
        
        s_Stopwatch.Reset();

        if ((option & RunTestOption.Sequence) != 0)
        {
            double[] eachTicks = new double[iterations];

            for (int i = 0; i < iterations; i++)
            {
                double old = s_Stopwatch.Elapsed.TotalSeconds;
                s_Stopwatch.Start();
                testAction();
                s_Stopwatch.Stop();
                eachTicks[i] = s_Stopwatch.Elapsed.TotalSeconds - old;
            }
            
            Array.Sort(eachTicks);
            int takeCount = (iterations - (iterations >> 2)).Min(1);
            double fast75Ticks = 0;
            for (int i = 0; i < takeCount; i++) fast75Ticks += eachTicks[i];

            s_StringBuilder.Append($"Time Cost Sequence({iterations} Times, fastest 75%): ");
            for (int i = 0; i < takeCount; i++) s_StringBuilder.Append($"{eachTicks[i].NumberString("F1")}s ");
            s_StringBuilder.AppendLine();
            s_StringBuilder.AppendLine($"75%({takeCount}) total : {fast75Ticks.NumberString("F1")}s");
            s_StringBuilder.AppendLine($"75%({takeCount}) average: {(fast75Ticks / takeCount).NumberString("F1")}s");
        }
        else
        {
            s_Stopwatch.Restart();
            for (int i = 0; i < iterations; i++) testAction();
            s_Stopwatch.Stop();

            s_StringBuilder.AppendLine($"{iterations} Times Costs: {s_Stopwatch.Elapsed.TotalSeconds.NumberString("F1")}s");
            s_StringBuilder.AppendLine($"Each Cost: {(s_Stopwatch.Elapsed.TotalSeconds / iterations).NumberString("F1")}s");
        }

        s_StringBuilder.AppendLine($"{iterations} Times Costs: {s_Stopwatch.Elapsed.TotalSeconds.NumberString("F1")}s");
        s_StringBuilder.AppendLine($"Each Cost: {(s_Stopwatch.Elapsed.TotalSeconds / iterations).NumberString("F1")}s");
        
        Console.WriteLine(s_StringBuilder.ToString());
    }
    
    [Flags]
    public enum RunTestOption: byte
    {
        Warm         = 0b0000_0001,
        Sequence     = 0b0000_0010,
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