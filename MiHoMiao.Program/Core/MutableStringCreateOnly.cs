using System.Runtime.CompilerServices;
using System.Text;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Program.Core;

public class MutableStringCreateOnly
{

    /*
        ------------------------------MutableString------------------------------
        Time Cost Sequence(5 Times): 1364.3ns 1838.4ns 1391.7ns 1472.1ns 1708.9ns 
        5 Times Costs: 7775.4ns
        Each Cost: 1555.080ns
    */
    public static void TestMutableStringSpeed() => TimeTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------InterpolatedString------------------------------
        Time Cost Sequence(5 Times): 417.6ns 480.3ns 462.6ns 451.1ns 448.6ns 
        5 Times Costs: 2260.2ns
        Each Cost: 452.040ns
     */
    public static void TestInterpolatedStringSpeed() => TimeTest.RunTest(
        InterpolatedStringAction, nameof(InterpolatedString), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------StringBuilder------------------------------
        Time Cost Sequence(5 Times): 833.6ns 1557.2ns 677.6ns 599.0ns 1134.3ns 
        5 Times Costs: 4801.7ns
        Each Cost: 960.340ns
    */
    public static void TestStringBuilderSpeed() => TimeTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Time Cost Sequence(5 Times): 1041.5ns 628.0ns 1087.4ns 401.9ns 569.6ns 
        5 Times Costs: 3728.4ns
        Each Cost: 745.680ns
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
        ------------------------------InterpolatedString------------------------------
        Memory Usage Sequence(5 Times): 2048.02 KB 2048.68 KB 2048.68 KB 2048.68 KB 2048.68 KB 
        5 Times Memory Usage: 10242.74 KB
        Each Memory Usage: 2048.548 KB
    */
    public static void TestInterpolatedStringMemory() => MemoryTest.RunTest(
        InterpolatedStringAction, nameof(InterpolatedString), 5,
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
    
    /*
        无结果
     */
    public static void TestStringConcatMemory() => MemoryTest.RunTest(
        StringConcatAction, nameof(string.Concat), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    public static void TestStringFormatMemory() => MemoryTest.RunTest(
        StringFormatAction, nameof(string.Format), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );

    private static void StringConcatAction()
    {
        string ms = "";
        for (int i = 0; i < 100000; i++) ms = string.Concat(ms, 'A', i, "test");
    }
    
    private static void StringFormatAction()
    {
        string ms = "";
        for (int i = 0; i < 100000; i++) ms = string.Format("{0}{1}{2}{3}", ms, 'A', i, "test");
    }
    
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
    
    private static void InterpolatedStringAction()
    {
        using InterpolatedString ms = new InterpolatedString(0x10_00_00);
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