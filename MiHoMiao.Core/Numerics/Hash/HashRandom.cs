//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.Core.Numerics.Hash;

/// <summary>
/// 基于哈希的伪随机数生成器, 提供多种类型的随机数生成方法.
/// 该类通过哈希算法生成确定性的随机数序列, 适用于需要可重复随机序列的场景.
/// </summary>
public class HashRandom(int value) : Random
{

    /// <summary>
    /// 初始种子值
    /// </summary>
    public int InitialSeed { get; } = value;

    /// <summary>
    /// 获取经过哈希计算的值
    /// </summary>
    public int Value { get => field = HashCodes.Combine(field); private set; } = value;

    public override int Next() => Value.Int32().Abs();

    public override int Next(int maxValue) => Value.Range(0, maxValue);

    public override int Next(int minValue, int maxValue)=> Value.Range(minValue, maxValue);
    
    public override long NextInt64() => (((long)(uint)Value.Int32() << 32) | (uint)Value.Int32()).Abs();

    public override long NextInt64(long maxValue) 
        => (((long)(uint)Value.Int32() << 32) | (uint)Value.Int32()).CircleClamp(0, maxValue);
    
    public override long NextInt64(long minValue, long maxValue)
        => (((long)(uint)Value.Int32() << 32) | (uint)Value.Int32()).CircleClamp(minValue, maxValue);

    public override float NextSingle() => Value.Range(0.0f, 1.0f);

    protected override double Sample() => Value.Range(0.0, 1.0);
    
    public override double NextDouble() => Value.Range(0.0, 1.0);

    /// <summary>
    /// 生成随机布尔值
    /// </summary>
    public bool NextBool(double chance) => NextDouble() < chance;

    public override void NextBytes(byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        NextBytes(buffer.AsSpan());
    }
    
    public override void NextBytes(Span<byte> buffer)
    {
        // 使用Unsafe直接写入内存
        ref byte bufferRef = ref MemoryMarshal.GetReference(buffer);
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


    /// <summary>
    /// 重置随机种子至初始值.
    /// </summary>
    public void ResetSeed() => Value = InitialSeed;
    
}
