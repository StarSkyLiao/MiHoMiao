using System.Runtime.CompilerServices;
using System.Text;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Program.Core;

public class MutableStringCreateOnly
{

    /*
        ------------------------------MutableString------------------------------
        Time Cost Sequence(5 Times): 3404.6ns 3876.8ns 3329.7ns 3322.1ns 3549.9ns 
        5 Times Costs: 17483.1ns
        Each Cost: 3496.620ns
    */
    public static void TestMutableStringSpeed() => TimeTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------StringBuilder------------------------------
        Time Cost Sequence(5 Times): 1173.4ns 1663.9ns 1049.2ns 974.7ns 1420.3ns 
        5 Times Costs: 6281.5ns
        Each Cost: 1256.300ns
    */
    public static void TestStringBuilderSpeed() => TimeTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Time Cost Sequence(5 Times): 1643.8ns 808.6ns 1145.3ns 885.9ns 915.6ns 
        5 Times Costs: 5399.2ns
        Each Cost: 1079.840ns
    */
    public static void TestDefaultInterpolatedStringHandlerSpeed() => TimeTest.RunTest(
        DefaultInterpolatedStringHandlerAction, nameof(DefaultInterpolatedStringHandler), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------MutableString------------------------------
        Memory Usage Sequence(5 Times): 2048.05 KB 2048.38 KB 2048.38 KB 2048.38 KB 2048.38 KB 
        5 Times Memory Usage: 10241.55 KB
        Each Memory Usage: 2048.309 KB
    */
    public static void TestMutableStringMemory() => MemoryTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );

    /*
        ------------------------------StringBuilder------------------------------
        Memory Usage Sequence(5 Times): 2048.07 KB 2048.40 KB 2048.07 KB 2048.40 KB 2048.40 KB
        5 Times Memory Usage: 10241.34 KB
        Each Memory Usage: 2048.267 KB
    */
    public static void TestStringBuilderMemory() => MemoryTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Memory Usage Sequence(5 Times): 8192.02 KB 8192.35 KB 8192.35 KB 8192.35 KB 8192.35 KB 
        5 Times Memory Usage: 40961.43 KB
        Each Memory Usage: 8192.286 KB
    */
    public static void TestDefaultInterpolatedStringHandlerMemory() => MemoryTest.RunTest(
        DefaultInterpolatedStringHandlerAction, nameof(DefaultInterpolatedStringHandler), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );

    private static void MutableStringAction()
    {
        using MutableString ms = new MutableString(0x10_00_00);
        for (int i = 0; i < 100000; i++)
        {
            ms.Append('A');
            ms.Append(i);
            ms.Append("test");
        }
    }
    
    private static void StringBuilderAction()
    {
        StringBuilder sb = new StringBuilder(0x10_00_00);
        for (int i = 0; i < 100000; i++)
        {
            sb.Append('A');
            sb.Append(i);
            sb.Append("test");
        }
    }
    
    private static void DefaultInterpolatedStringHandlerAction()
    {
        DefaultInterpolatedStringHandler sb = new DefaultInterpolatedStringHandler(0, 3 * 100000);
        for (int i = 0; i < 100000; i++)
        {
            sb.AppendFormatted('A');
            sb.AppendFormatted(i);
            sb.AppendFormatted("test");
        }
    }

}