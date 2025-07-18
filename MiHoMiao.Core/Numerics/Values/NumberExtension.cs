//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Numerics.Values;

public static class NumberExtension
{
    /// <summary>
    /// 返回数字的绝对值
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Abs<T>(this T value) where T : INumber<T> => value < T.Zero ? -value : value;
    
    /// <summary>
    /// 返回数字的符号
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign<T>(this T value) where T : INumber<T> 
        => value < T.Zero ? -1 : value > T.Zero ? 1 : 0;
    
    /// <summary>
    /// 返回数字的指定幂
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Pow<T>(this T value, int power) where T : INumber<T>
    {
        T result = T.One;
        while (power > 0)
        {
            if ((power & 1) == 1) result *= value;
            value *= value;
            power >>= 1;
        }
        return result;
    }

    /// <summary>
    /// 将给定的数字限制在指定数字之上.
    /// 换言之, 最小为 min
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Min<T>(this T value, T min) where T : INumber<T> => value < min ? min : value;

    /// <summary>
    /// 将给定的数字限制在指定数字之下.
    /// 换言之, 最大为 max
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Max<T>(this T value, T max) where T : INumber<T> => value > max ? max : value;

    /// <summary>
    /// 将指定的数字限制在 0 ~ 1 范围内，超出的部分被截断
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Clamp01<T>(this T value) where T : INumber<T>
        => value < T.Zero ? T.Zero : value > T.One ? T.One : value;
    
    /// <summary>
    /// 将给定的数字限制在指定范围内, 超出的部分被截断
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Clamp<T>(this T value, T min, T max) where T : INumber<T>
    {
        if (value < min) value = min;
        else if (value > max) value = max;
        return value;
    }
    
    /// <summary>
    /// 将给定的数字限制在指定范围内, 并支持循环回绕;
    /// 如果值超出范围, 会自动回绕到范围内的相应位置.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T CircleClamp<T>(this T value, T leftInclude, T rightExclude) where T : INumber<T>
    {
        T range = rightExclude - leftInclude;
        value = (value - leftInclude) % range;
        if (value < T.Zero) value += range;
        return value + leftInclude;
    }
    
    /// <summary>
    /// 返回 rate 对应的 T 类型的数字
    /// </summary>
    public static T Number<T>(uint rate) where T : INumber<T> => T.CreateChecked(rate);
    
}