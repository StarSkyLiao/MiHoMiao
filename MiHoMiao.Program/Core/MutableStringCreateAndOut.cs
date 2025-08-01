using System.Runtime.CompilerServices;
using System.Text;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Program.Core;

public class MutableStringCreateAndOut
{

    /*
        ------------------------------MutableString------------------------------
        Time Cost Sequence(5 Times): 7434.0ns 8256.5ns 7101.8ns 5718.3ns 5914.5ns 
        5 Times Costs: 34425.1ns
        Each Cost: 6885.020ns
    */
    public static void TestMutableStringSpeed() => TimeTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------StringBuilder------------------------------
        Time Cost Sequence(5 Times): 4179.4ns 4148.6ns 4016.0ns 2424.9ns 9321.8ns 
        5 Times Costs: 24090.7ns
        Each Cost: 4818.140ns
    */
    public static void TestStringBuilderSpeed() => TimeTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Time Cost Sequence(5 Times): 5469.5ns 5385.5ns 5205.0ns 3778.8ns 4220.7ns 
        5 Times Costs: 24059.5ns
        Each Cost: 4811.900ns
    */
    public static void TestDefaultInterpolatedStringHandlerSpeed() => TimeTest.RunTest(
        DefaultInterpolatedStringHandlerAction, nameof(DefaultInterpolatedStringHandler), 5,
        TimeTest.RunTestOption.Warm | TimeTest.RunTestOption.Sequence
    );

    /*
        ------------------------------MutableString------------------------------
        Memory Usage Sequence(5 Times): 6735.55 KB 6735.88 KB 6735.88 KB 6735.88 KB 6735.88 KB 
        5 Times Memory Usage: 33679.05 KB
        Each Memory Usage: 6735.809 KB
    */
    public static void TestMutableStringMemory() => MemoryTest.RunTest(
        MutableStringAction, nameof(MutableString), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );

    /*
        ------------------------------StringBuilder------------------------------
        Memory Usage Sequence(5 Times): 8705.52 KB 8705.84 KB 8705.84 KB 8705.84 KB 8705.84 KB 
        5 Times Memory Usage: 43528.89 KB
        Each Memory Usage: 8705.778 KB
    */
    public static void TestStringBuilderMemory() => MemoryTest.RunTest(
        StringBuilderAction, nameof(StringBuilder), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );
    
    /*
        ------------------------------DefaultInterpolatedStringHandler------------------------------
        Memory Usage Sequence(5 Times): 14849.99 KB 14850.65 KB 14850.65 KB 14850.65 KB 14850.65 KB 
        5 Times Memory Usage: 74252.59 KB
        Each Memory Usage: 14850.517 KB
    */
    public static void TestDefaultInterpolatedStringHandlerMemory() => MemoryTest.RunTest(
        DefaultInterpolatedStringHandlerAction, nameof(DefaultInterpolatedStringHandler), 5,
        MemoryTest.RunTestOption.Warm | MemoryTest.RunTestOption.Sequence
    );

    
    private static readonly StringBuilder s_Buffer = new StringBuilder(0x10_00_00);
    private static void MutableStringAction()
    {
        using MutableString ms = new MutableString(0x10_00_00);
        for (int i = 0; i < 100000; i++)
        {
            ms.Clear();
            ms.Append('A');
            ms.Append(i);
            ms.Append("test");
            s_Buffer.Append(ms.ToString());
        }

        s_Buffer.Clear();
    }
    
    private static void StringBuilderAction()
    {
        StringBuilder sb = new StringBuilder(0x10_00_00);
        for (int i = 0; i < 100000; i++)
        {
            sb.Clear();
            sb.Append('A');
            sb.Append(i);
            sb.Append("test");
            s_Buffer.Append(sb.ToString());
        }
        s_Buffer.Clear();
    }
    
    private static void DefaultInterpolatedStringHandlerAction()
    {
        DefaultInterpolatedStringHandler sb = new DefaultInterpolatedStringHandler(0, 3 * 100000);
        for (int i = 0; i < 100000; i++)
        {
            sb.AppendFormatted('A');
            sb.AppendFormatted(i);
            sb.AppendFormatted("test");
            s_Buffer.Append(sb.ToStringAndClear());
        }
        s_Buffer.Clear();
    }

}