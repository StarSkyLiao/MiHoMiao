namespace MiHoMiao.Core.Diagnostics;

[Flags]
public enum PerfTestOption: uint
{
    None         = 0,
    TimeTest     = 0b1 << 0, // 需要测试时间开销
    MemoryTest   = 0b1 << 1, // 需要测试内存开销
    
    Warm         = 0b1 << 4, // 测试前, 先进行一次预热
    Sequence     = 0b1 << 5, // 将结果按照一个有序序列全部打印出来
    Best75       = 0b1 << 6, // 单独考量表现最好的 75% 测试用例
    Simple       = 0b1 << 7, // 简化测试结果, 只展示最核心的数据
    NoGc         = 0b1 << 8, // 简化测试结果, 只展示最核心的数据
}