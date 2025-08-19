using System.Runtime.CompilerServices;
using System.Text;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Program.Core;

public class MutableStringCreateAndOut
{

    /*
        ------------------------------MutableString------------------------------
        Time Cost Sequence(5 Times): 1369.6ns 1394.5ns 1373.9ns 1370.6ns 1382.7ns 
        5 Times Costs: 6891.3ns
        Each Cost: 1378.260ns
    */
    public static void TestMutableStringSpeed() => TimeTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------InterpolatedString------------------------------
        Time Cost Sequence(20 Times): 3628.5ns 3637.3ns 3281.1ns 1630.6ns 1789.6ns 1866.5ns 2130.0ns 1615.5ns 1688.8ns 1866.4ns 1884.6ns 1679.9ns 1624.1ns 1867.0ns 1658.9ns 1620.9ns 1723.4ns 1670.2ns 1793.9ns 1639.5ns 
        20 Times Costs: 40296.7ns
        Each Cost: 1749.988ns
    */
    public static void TestInterpolatedStringSpeed() => TimeTest.RunTest(
        InterpolatedStringAction, nameof(InterpolatedString), 20,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------StringBuilder------------------------------
        Time Cost Sequence(20 Times): 3761.6ns 3584.0ns 3100.6ns 1840.3ns 1608.4ns 1653.0ns 2173.6ns 1605.0ns 1610.9ns 1841.4ns 1629.2ns 1615.1ns 1774.6ns 1838.2ns 1621.6ns 1770.2ns 1752.6ns 1613.5ns 1565.7ns 1613.3ns 
        20 Times Costs: 39572.8ns
        Each Cost: 1713.353ns
    */
    public static void TestStringBuilderSpeed() => TimeTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 20,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Time Cost Sequence(5 Times): 13048.5ns 13065.3ns 12515.7ns 11093.2ns 11411.9ns 
        5 Times Costs: 61134.6ns
        Each Cost: 12226.920ns
    */
    public static void TestDefaultInterpolatedStringHandlerSpeed() => TimeTest.RunTest(
        DefaultInterpolatedStringHandlerAction, nameof(DefaultInterpolatedStringHandler), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------Concat------------------------------
        Time Cost Sequence(5 Times): 5592.8ns 3833.4ns 3368.6ns 3801.7ns 3977.4ns 
        5 Times Costs: 20573.9ns
        Each Cost: 4114.780ns
    */
    public static void TestStringConcatSpeed() => TimeTest.RunTest(
        StringConcatAction, nameof(string.Concat), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------StackallocAction------------------------------
        Time Cost Sequence(20 Times): 3420.6ns 3709.7ns 4509.1ns 1449.5ns 1499.3ns 1470.2ns 1952.7ns 1410.9ns 1534.6ns 1667.9ns 1490.4ns 1420.5ns 1417.2ns 1560.8ns 1406.8ns 1409.1ns 1547.4ns 1469.2ns 1412.0ns 1523.0ns 
        20 Times Costs: 37280.9ns
        Each Cost: 1508.406ns
    */
    public static void TestStackallocActionSpeed() => TimeTest.RunTest(
        StackallocAction, nameof(StackallocAction), 20,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------StringFormatAction------------------------------
        Time Cost Sequence(20 Times): 10188.9ns 6684.2ns 7073.6ns 6170.4ns 6430.8ns 7807.4ns 6931.5ns 6265.6ns 6286.6ns 6240.1ns 6181.0ns 6466.9ns 6170.1ns 6449.2ns 6194.5ns 6433.9ns 6471.3ns 6114.0ns 6370.9ns 6466.0ns 
        20 Times Costs: 133396.9ns
        Each Cost: 6484.632ns
    */
    public static void TestStringFormatActionSpeed() => TimeTest.RunTest(
        StringFormatAction, nameof(StringFormatAction), 20,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
        
    /*
        ------------------------------MutableString------------------------------
        Memory Usage Sequence(5 Times): 0.20 KB 0.20 KB 0.20 KB 0.20 KB 0.20 KB 
        5 Times Memory Usage: 0.98 KB
        Each Memory Usage: 0.195 KB
    */
    public static void TestMutableStringMemory() => MemoryTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------InterpolatedString------------------------------
        Memory Usage Sequence(5 Times): 4610.09 KB 4609.43 KB 4610.09 KB 4609.80 KB 4609.43 KB 
        5 Times Memory Usage: 23048.84 KB
        Each Memory Usage: 4609.767 KB
    */
    public static void TestInterpolatedStringMemory() => MemoryTest.RunTest(
        InterpolatedStringAction, nameof(InterpolatedString), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------StringBuilder------------------------------
        Memory Usage Sequence(5 Times): 4617.45 KB 4617.77 KB 4617.77 KB 4617.77 KB 4617.77 KB 
        5 Times Memory Usage: 23088.54 KB
        Each Memory Usage: 4617.708 KB
    */
    public static void TestStringBuilderMemory() => MemoryTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Memory Usage Sequence(5 Times): 4609.90 KB 4610.55 KB 4611.08 KB 4610.55 KB 4610.55 KB 
        5 Times Memory Usage: 23052.64 KB
        Each Memory Usage: 4610.528 KB
    */
    public static void TestDefaultInterpolatedStringHandlerMemory() => MemoryTest.RunTest(
        DefaultInterpolatedStringHandlerAction, nameof(DefaultInterpolatedStringHandler), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------Concat------------------------------
        Memory Usage Sequence(5 Times): 14756.25 KB 14756.58 KB 14756.58 KB 14756.58 KB 14756.58 KB
        5 Times Memory Usage: 73782.56 KB
        Each Memory Usage: 14756.513 KB
    */
    public static void TestStringConcatMemory() => MemoryTest.RunTest(
        StringConcatAction, nameof(string.Concat), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------StackallocAction------------------------------
        Memory Usage Sequence(5 Times): 4609.38 KB 4609.70 KB 4609.70 KB 4609.70 KB 4609.70 KB 
        5 Times Memory Usage: 23048.19 KB
        Each Memory Usage: 4609.637 KB
    */
    public static void TestStackallocActionMemory() => MemoryTest.RunTest(
        StackallocAction, nameof(StackallocAction), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------StringFormatAction------------------------------
        Memory Usage Sequence(5 Times): 9296.88 KB 9296.88 KB 9296.88 KB 9296.88 KB 9296.88 KB 
        5 Times Memory Usage: 46484.38 KB
        Each Memory Usage: 9296.875 KB
    */
    public static void TestStringFormatActionMemory() => MemoryTest.RunTest(
        StringFormatAction, nameof(StringFormatAction), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    private static void StringConcatAction()
    {
        for (int i = 0; i < 100000; i++) string.Concat('A', i, "test").CopyTo(s_Buffer);
    }
    
    private static void StringFormatAction()
    {
        for (int i = 0; i < 100000; i++) string.Format("{0}{1}{2}", 'A', i, "test").CopyTo(s_Buffer);
    }

    private const string TestString = "test";
    
    private static readonly char[] s_Buffer = new char[4096];
    public static void MutableStringAction()
    {
        using MutableString ms = new MutableString(16);
        for (int i = 0; i < 100000; i++)
        {
            ms.Clear();
            ms.Append('A');
            ms.Append(i);
            ms.Append(TestString);
            ms.Read().CopyTo(s_Buffer);
        }
    }
    
    private static void InterpolatedStringAction()
    {
        using InterpolatedString ms = new InterpolatedString(16);
        for (int i = 0; i < 100000; i++)
        {
            ms.SafeClear();
            ms.Append('A');
            ms.Append(i);
            ms.Append(TestString);
            ms.ToString().CopyTo(s_Buffer);
        }
    }
    
    private static void StringBuilderAction()
    {
        StringBuilder sb = new StringBuilder(0x10_00);
        for (int i = 0; i < 100000; i++)
        {
            sb.Clear();
            sb.Append('A');
            sb.Append(i);
            sb.Append(TestString);
            sb.ToString().CopyTo(s_Buffer);
        }
    }
    
    private static void DefaultInterpolatedStringHandlerAction()
    {
        DefaultInterpolatedStringHandler sb = new DefaultInterpolatedStringHandler(0, 3);
        for (int i = 0; i < 100000; i++)
        {
            sb.AppendFormatted('A');
            sb.AppendFormatted(i);
            sb.AppendFormatted(TestString);
            sb.ToStringAndClear().CopyTo(s_Buffer);
        }
    }
    
    private static unsafe void StackallocAction()
    {
        Span<char> ms = stackalloc char[16];
        for (int i = 0; i < 100000; i++)
        {
            int index = 0;
            ms[index++] = 'A';
            i.TryFormat(ms[index..], out int delta);
            index += delta;
            TestString.CopyTo(ms[index..]);
            index += 4;
            ms[..index].ToString().CopyTo(s_Buffer);
        }
    }

}