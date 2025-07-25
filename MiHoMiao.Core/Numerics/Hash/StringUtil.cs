using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MiHoMiao.Core.Numerics.Hash;

public static class StringUtil
{
    /// <summary>
    /// 计算输入字符跨度的 32 位哈希值，使用 Marvin32 算法。
    /// </summary>
    /// <param name="value">输入的字符跨度</param>
    /// <returns>计算得到的 32 位哈希值</returns>
    public static int StringHash(this ReadOnlySpan<char> value)
    {
        const uint Seed = HashCodes.Seed; // 使用常量种子值初始化哈希计算
        // 将字符跨度转换为字节跨度（每个 char 占 2 字节），并计算总字节数
        // 注意：这里不会发生溢出，因为输入长度是 int 类型，转换为 uint 后乘以 2 仍安全
        return ComputeHash32(
            ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(value)),
            (uint)value.Length * 2 /* in bytes, not chars */, Seed, 0
        );
    }
    
    /// <summary>
    /// 计算字节数据的 32 位 Marvin32 哈希值。
    /// </summary>
    /// <param name="data">指向字节数据的引用</param>
    /// <param name="count">字节数据的长度（以字节为单位）</param>
    /// <param name="p0">哈希计算的第一个种子值</param>
    /// <param name="p1">哈希计算的第二个种子值</param>
    /// <returns>计算得到的 32 位哈希值</returns>
    private static int ComputeHash32(ref byte data, uint count, uint p0, uint p1)
    {
        // 控制流从上到下，优先处理 >= 8 字节（4 个字符）的输入，减少分支
        // 如果输入 < 8 字节，会跳转到处理小输入的逻辑

        if (count < 8)
        {
            // 输入不足 8 字节，无法进入主循环
            // 如果有 4 到 7 字节，则跳转到处理 4 到 7 字节的逻辑
            if (count >= 4) goto Between4And7BytesRemain;

            // 输入过小（0 到 3 字节），跳转到末尾的小输入处理逻辑
            goto InputTooSmallToEnterMainLoop;
        }

        // 主循环 - 每次读取 8 字节数据
        // 每次循环展开两次 Block 函数调用以优化性能

        uint loopCount = count / 8; // 计算需要循环的次数（每 8 字节一次）

        do
        {
            // 并行读取两个 32 位值（共 8 字节），以利用 x86 的双端口读取优势
            // 避免使用 64 位读取，因为字符串数据在 64 位进程中通常不是 8 字节对齐
            p0 += Unsafe.ReadUnaligned<uint>(ref data);
            uint nextUInt32 = Unsafe.ReadUnaligned<uint>(ref Unsafe.AddByteOffset(ref data, 4));

            // 对读取的两个 32 位值各执行一次 Block 函数，总共两次
            Block(ref p0, ref p1);
            p0 += nextUInt32;
            Block(ref p0, ref p1);

            // 更新数据指针，移动 8 字节
            // 使用递减循环以优化 JIT 代码生成（相比增量循环）
            data = ref Unsafe.AddByteOffset(ref data, 8);
        } while (--loopCount > 0);

        // 主循环结束后，count 的低 3 位表示剩余字节数（0 到 7 字节）
        // 检查是否需要处理 4 到 7 字节的剩余数据
        if ((count & 0b_0100) == 0) goto DoFinalPartialRead;

        Between4And7BytesRemain:

        // 如果剩余 4 到 7 字节（或初始输入就是 4 到 7 字节），读取 4 字节并执行一次 Block 函数
        p0 += Unsafe.ReadUnaligned<uint>(ref data);
        Block(ref p0, ref p1);

        DoFinalPartialRead:

        // 处理剩余 0 到 3 字节
        // 读取缓冲区末尾的 4 字节（可能包含已处理过的字节）
        uint partialResult =
            Unsafe.ReadUnaligned<uint>(ref Unsafe.Add(ref Unsafe.AddByteOffset(ref data, (nuint)count & 7), -4));

        // 根据剩余字节数调整 partialResult，确保只保留未处理的数据
        // 示例（小端序）：
        // count % 4 = 0 -> [ ## ## ## ## ] -> 0x####_#### -> 0x0000_0080
        // count % 4 = 1 -> [ ## ## ## AA ] -> 0xAA##_#### -> 0x0000_80AA
        // count % 4 = 2 -> [ ## ## AA BB ] -> 0xBBAA_#### -> 0x0080_BBAA
        // count % 4 = 3 -> [ ## AA BB CC ] -> 0xCCBB_AA## -> 0x80CC_BBAA

        count = ~count << 3;

        if (BitConverter.IsLittleEndian)
        {
            partialResult >>= 8; // 为 0x80 字节腾出空间
            partialResult |= 0x8000_0000u; // 在开头添加 0x80
            partialResult >>= (int)count & 0x1F; // 移出已处理过的字节
        }
        else
        {
            partialResult <<= 8; // 为 0x80 字节腾出空间
            partialResult |= 0x80u; // 在末尾添加 0x80
            partialResult <<= (int)count & 0x1F; // 移出已处理过的字节
        }

        DoFinalRoundsAndReturn:

        // 合并最终的 partialResult，并执行两次 Block 函数以完成 Marvin 算法
        p0 += partialResult;
        Block(ref p0, ref p1);
        Block(ref p0, ref p1);

        return (int)(p1 ^ p0); // 返回最终哈希值（p1 与 p0 异或）

        InputTooSmallToEnterMainLoop:

        // 处理 0 到 3 字节的小输入
        // 初始化 partialResult 为“无数据”状态
        if (BitConverter.IsLittleEndian)
            partialResult = 0x80u;
        else
            partialResult = 0x80000000u;

        if ((count & 0b_0001) != 0)
        {
            // 如果输入为 1 或 3 字节，读取 1 个字节并合并到 partialResult
            // 示例（小端序/大端序）：
            // [ AA ] -> 0x0000_80AA / 0xAA80_0000
            // [ AA BB CC ] -> 0x0000_80CC / 0xCC80_0000
            partialResult = Unsafe.AddByteOffset(ref data, (nuint)count & 2);

            if (BitConverter.IsLittleEndian)
            {
                partialResult |= 0x8000;
            }
            else
            {
                partialResult <<= 24;
                partialResult |= 0x800000u;
            }
        }

        if ((count & 0b_0010) != 0)
        {
            // 如果输入为 2 或 3 字节，读取 1 个 ushort 并合并到 partialResult
            // 示例（小端序/大端序）：
            // [ AA BB ] -> 0x0080_BBAA / 0xAABB_8000
            // [ AA BB CC ] -> 0x80CC_BBAA / 0xAABB_CC80
            if (BitConverter.IsLittleEndian)
            {
                partialResult <<= 16;
                partialResult |= Unsafe.ReadUnaligned<ushort>(ref data);
            }
            else
            {
                partialResult |= Unsafe.ReadUnaligned<ushort>(ref data);
                partialResult = BitOperations.RotateLeft(partialResult, 16);
            }
        }

        // 所有数据处理完毕，跳转到最终轮次
        goto DoFinalRoundsAndReturn;
    }
    
    /// <summary>
    /// 执行 Marvin32 算法的单轮 Block 操作，更新哈希状态。
    /// </summary>
    /// <param name="rp0">第一个哈希状态（p0）。</param>
    /// <param name="rp1">第二个哈希状态（p1）。</param>
    private static void Block(ref uint rp0, ref uint rp1)
    {
        // 使用临时变量以避免直接修改输入
        uint p0 = rp0;
        uint p1 = rp1;

        // Marvin32 算法的核心变换：异或、左旋、加法
        p1 ^= p0;
        p0 = BitOperations.RotateLeft(p0, 20);

        p0 += p1;
        p1 = BitOperations.RotateLeft(p1, 9);

        p1 ^= p0;
        p0 = BitOperations.RotateLeft(p0, 27);

        p0 += p1;
        p1 = BitOperations.RotateLeft(p1, 19);

        // 更新引用参数
        rp0 = p0;
        rp1 = p1;
    }
}