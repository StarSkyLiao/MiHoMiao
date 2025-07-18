//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MiHoMiao.Core.Numerics.Hash;

/// <summary>
/// 基于哈希的伪随机数生成器, 提供多种类型的随机数生成方法.
/// 该类通过哈希算法生成确定性的随机数序列, 适用于需要可重复随机序列的场景.
/// </summary>
public class HashRandom(int value)
{
    /// <summary>
    /// 获取经过哈希计算的值
    /// </summary>
    public int Value { get => field = HashCodes.Combine(field); } = value;
    
    /// <summary>
    /// 生成指定范围内的随机整数
    /// </summary>
    public int Range(int minValue, int maxValue) => Value.Range(minValue, maxValue);

    /// <summary>
    /// 生成指定范围内的随机整数
    /// </summary>
    public float Single(float minValue, float maxValue) => Value.Range(minValue, maxValue);

    /// <summary>
    /// 生成指定范围内的随机双精度浮点数
    /// </summary>
    public double Double(double minValue, double maxValue) => Value.Range(minValue, maxValue);

    /// <summary>
    /// 生成一个随机32位整数
    /// </summary>
    public int Int32() => Value.Int32();

    /// <summary>
    /// 生成 [0, 1) 之间的随机单精度浮点数
    /// </summary>
    public float NextSingle() => Single(0.0f, 1.0f);

    /// <summary>
    /// 生成 [0, 1) 之间的随机双精度浮点数
    /// </summary>
    public double NextDouble() => Double(0.0, 1.0);

    /// <summary>
    /// 生成随机布尔值
    /// </summary>
    public bool NextBool(double chance) => Double(0, 1) < chance;

    /// <summary>
    /// 填充字节数组
    /// </summary>
    public void NextBytes(byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        // 使用Unsafe直接写入内存
        ref byte bufferRef = ref MemoryMarshal.GetReference(buffer.AsSpan());
        int i = 0;

        // 批量填充4字节（int大小）
        while (i + 4 <= buffer.Length)
        {
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref bufferRef, i), Value);
            i += 4;
        }

        // 处理剩余字节
        if (i >= buffer.Length) return;
        int value = Value;
        for (int j = 0; j < buffer.Length - i; j++)
        {
            Unsafe.Add(ref bufferRef, i + j) = (byte)(value >> (j * 8));
        }
    }
}
