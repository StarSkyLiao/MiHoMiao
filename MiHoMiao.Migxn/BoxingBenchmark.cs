using BenchmarkDotNet.Attributes;

namespace MiHoMiao.Migxn;

[MemoryDiagnoser(displayGenColumns: true)]   // 显示各代 GC 次数 + 分配量
public class BoxingBenchmark
{
    // 自定义结构体
    private readonly record struct Point(int X, int Y)
    {
        public int Sum() => X + Y;
    }

    [Benchmark(Baseline = true, Description = "纯值类型（无装箱）")]
    public int Direct()
    {
        Point p = new Point(1, 2);
        return p.Sum();
    }

    [Benchmark(Description = "赋值给 ValueType（装箱）")]
    public int ViaValueType()
    {
        ValueType v = new Point(1, 2);   // 这里发生装箱
        return ((Point)v).Sum();         // 拆箱后再调用
    }

    [Benchmark(Description = "赋值给 object（装箱）")]
    public int ViaObject()
    {
        object o = new Point(1, 2);      // 装箱
        return ((Point)o).Sum();         // 拆箱
    }
}