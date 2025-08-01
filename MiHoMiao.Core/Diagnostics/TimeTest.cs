//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace MiHoMiao.Core.Diagnostics;

/// <summary>
/// 用于测算一个方法的耗时
/// </summary>
public static class TimeTest
{
    private static readonly Stopwatch s_Stopwatch = new Stopwatch();
    
    /// <summary>
    /// 运行时间使用测试，测量指定方法的时间消耗量。
    /// </summary>
    public static void RunTest(Action testAction, string? name = null, int iterations = 1, RunTestOption option = 0)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("------------------------------");
        stringBuilder.Append($"{name ?? testAction.ToString()}");
        stringBuilder.AppendLine("------------------------------");
        
        if ((option & RunTestOption.Warm) != 0) testAction();
        
        s_Stopwatch.Reset();

        if ((option & RunTestOption.Sequence) != 0)
        {
            stringBuilder.Append($"Time Cost Sequence({iterations} Times): ");
            for (int i = 0; i < iterations; i++)
            {
                long old = s_Stopwatch.ElapsedTicks;
                s_Stopwatch.Start();
                testAction();
                s_Stopwatch.Stop();
                stringBuilder.Append($"{0.1 * (s_Stopwatch.ElapsedTicks - old):F1}ns ");
            }

            stringBuilder.AppendLine();
        }
        else
        {
            s_Stopwatch.Restart();
            for (int i = 0; i < iterations; i++) testAction();
            s_Stopwatch.Stop();
        }

        stringBuilder.AppendLine($"{iterations} Times Costs: {0.1 * s_Stopwatch.ElapsedTicks:F1}ns");
        stringBuilder.AppendLine($"Each Cost: {0.1 * s_Stopwatch.ElapsedTicks / iterations:F3}ns");
        
        Console.WriteLine(stringBuilder.ToString());
    }
    
    [Flags]
    public enum RunTestOption: byte
    {
        Warm         = 0b0000_0001,
        Sequence     = 0b0000_0010,
    }
    
}