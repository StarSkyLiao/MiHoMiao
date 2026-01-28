namespace MiHoMiao.Core.Diagnostics;

public class PerfTest
{
    public static void RunTest(Action testAction, string? name = null, int iterations = 1, PerfTestOption option = 0)
    {
        MemoryTest.RunTest(testAction, name, iterations, option);
        TimeTest.RunTest(testAction, name, iterations, option);
    }
}