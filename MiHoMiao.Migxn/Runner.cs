

using System.Diagnostics;
using System.Drawing;
using BenchmarkDotNet.Running;

namespace MiHoMiao.Migxn;

public static class Runner
{
    private const string Input =
        """
        fun Foo() : i32 -> {
            var a : r64 = 1
            var b : object = "1"
            a = 1 * a + 3
            while(a > 1 and a < 10) {
                if (a > 5) ret
                a = a + 1
            }
        }
        """;
    public static void Run()
    {
        BenchmarkRunner.Run<BoxingBenchmark>();
    }
    
}