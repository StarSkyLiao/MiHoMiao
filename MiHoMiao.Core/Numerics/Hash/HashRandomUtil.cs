//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.Core.Numerics.Hash;

/// <summary>
/// 使用一个或多个值作为种子，经过哈希运算后得出随机值。
/// 输入相同时，永远返回相同的运算结果。
/// 相比较于 Randomization 类，哈希运算使用的空间会更小，但运行效率会更低。
/// </summary>
public static class HashRandomUtil
{
    /// <summary>
    /// 根据输入的种子，返回 [minInclude,maxExclude) 之间的一个随机整数
    /// </summary>
    public static int Range<TSeed>(this TSeed seed, int minInclude, int maxExclude)
        => HashCodes.Combine(seed).CircleClamp(minInclude, maxExclude);
    
    /// <summary>
    /// 根据输入的种子，返回 [minInclude,maxExclude) 之间的一个随机整数
    /// </summary>
    public static float Range<TSeed>(this TSeed seed, float minInclude, float maxExclude) 
        => minInclude + Single(seed) * maxExclude;

    /// <summary>
    /// 根据输入的种子，返回 [minInclude,maxExclude) 之间的一个随机整数
    /// </summary>
    public static double Range<TSeed>(this TSeed seed, double minInclude, double maxExclude) 
        => minInclude + Double(seed) * maxExclude;

    /// <summary>
    /// 根据输入的种子，返回一个随机 Int32 类型值
    /// </summary>
    public static int Int32<T>(this T seed) => HashCodes.Combine(seed);

    /// <summary>
    /// 根据输入的种子，返回一个随机 Int32 类型值
    /// </summary>
    public static int Int32<T>(T seed1, T seed2) => HashCodes.Combine(seed1, seed2);

    /// <summary>
    /// 根据输入的种子，返回一个随机 Int32 类型值
    /// </summary>
    public static int Int32<T>(T seed1, T seed2, T seed3) => HashCodes.Combine(seed1, seed2, seed3);

    /// <summary>
    /// 根据输入的种子，返回 [0,1) 之间的一个随机 Single 值
    /// </summary>
    public static float Single<T>(this T seed)
        => (float)HashCodes.Combine(seed) / int.MaxValue / 2 + 0.5f;

    /// <summary>
    /// 根据输入的种子，返回 [0,1) 之间的一个随机 Single 值
    /// </summary>
    public static float Single<T>(T seed1, T seed2)
        => (float)HashCodes.Combine(seed1, seed2) / int.MaxValue / 2 + 0.5f;

    /// <summary>
    /// 根据输入的种子，返回 [0,1) 之间的一个随机 Single 值
    /// </summary>
    public static float Single<T>(T seed1, T seed2, T seed3)
        => (float)HashCodes.Combine(seed1, seed2, seed3) / int.MaxValue / 2 + 0.5f;

    /// <summary>
    /// 根据输入的种子，返回 [0,1) 之间的一个随机 Double 值
    /// </summary>
    public static double Double<T>(this T seed)
        => (double)HashCodes.Combine(seed) / int.MaxValue / 2 + 0.5;

    /// <summary>
    /// 根据输入的种子，返回 [0,1) 之间的一个随机 Double 值
    /// </summary>
    public static double Double<T>(T seed1, T seed2)
        => (double)HashCodes.Combine(seed1, seed2) / int.MaxValue / 2 + 0.5;

    /// <summary>
    /// 根据输入的种子，返回 [0,1) 之间的一个随机 Double 值
    /// </summary>
    public static double Double<T>(T seed1, T seed2, T seed3)
        => (double)HashCodes.Combine(seed1, seed2, seed3) / int.MaxValue / 2 + 0.5;

    /// <summary>
    /// 根据种子随机选取
    /// </summary>
    public static TOut? Select<TOut, TIn>(this IEnumerable<TOut> enumerable, TIn seed)
    {
        IList<TOut> array = enumerable as IList<TOut> ?? enumerable.ToArray();
        return array.Count == 0 ? default : array[Range(seed, 0, array.Count)];
    }

}