using System.Text;

namespace MiHoMiao.Core.Diagnostics;

/// <summary>
/// 用于测算一个方法的内存分配量
/// </summary>
public static class MemoryTest
{
    /// <summary>
    /// 运行内存使用测试，测量指定方法的内存分配量。
    /// </summary>
    public static void RunTest(Action testAction, string? name = null, int iterations = 1, RunTestOption option = 0)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("------------------------------");
        stringBuilder.Append($"{name ?? testAction.ToString()}");
        stringBuilder.AppendLine("------------------------------");

        // 强制 GC 清理以确保初始内存状态
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // 预热运行
        if ((option & RunTestOption.Warm) != 0)
        {
            testAction();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        long totalMemoryBytes = 0;
        if ((option & RunTestOption.Sequence) != 0)
        {
            stringBuilder.Append($"Memory Usage Sequence({iterations} Times): ");
            for (int i = 0; i < iterations; i++)
            {
                long memoryStart = GC.GetTotalAllocatedBytes(true);
                testAction();
                long memoryEnd = GC.GetTotalAllocatedBytes(true);
                long memoryUsed = memoryEnd - memoryStart;
                totalMemoryBytes += memoryUsed;
                stringBuilder.Append($"{memoryUsed / 1024.0:F2} KB ");
                // 清理内存以减少后续迭代的干扰
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            stringBuilder.AppendLine();
        }
        else
        {
            long memoryStart = GC.GetTotalAllocatedBytes(true);
            for (int i = 0; i < iterations; i++)
            {
                testAction();
            }
            long memoryEnd = GC.GetTotalAllocatedBytes(true);
            totalMemoryBytes = memoryEnd - memoryStart;
        }

        stringBuilder.AppendLine($"{iterations} Times Memory Usage: {totalMemoryBytes / 1024.0:F2} KB");
        stringBuilder.AppendLine($"Each Memory Usage: {totalMemoryBytes / 1024.0 / iterations:F3} KB");

        Console.WriteLine(stringBuilder.ToString());
    }

    [Flags]
    public enum RunTestOption : byte
    {
        /// <summary>
        /// 执行一次预热运行以减少 JIT 或缓存影响。
        /// </summary>
        Warm = 0b0000_0001,
        /// <summary>
        /// 顺序测量每次迭代的内存使用量。
        /// </summary>
        Sequence = 0b0000_0010,
    }
}