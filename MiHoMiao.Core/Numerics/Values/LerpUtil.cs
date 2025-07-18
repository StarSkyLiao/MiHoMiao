//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------ 

using System.Numerics;

namespace MiHoMiao.Core.Numerics.Values;

public static class LerpUtil
{
    
    /// <summary>
    /// 返回两个数字之间线性平滑的结果
    /// </summary>
    public static T Linear<T>(T start, T end, T progress) where T : INumber<T>
        => start + (end - start) * progress;

    /// <summary>
    /// 返回两个数字之间三次多项式平滑的结果
    /// </summary>
    public static T Smooth<T>(T start, T end, T progress) where T : INumber<T>
        => start + (end - start) *
            (progress * progress * (NumberExtension.Number<T>(3) - NumberExtension.Number<T>(2) * progress));

    /// <summary>
    /// 返回两个数字之间五次多项式平滑的结果
    /// </summary>
    public static T Quintic<T>(T start, T end, T progress) where T : INumber<T>
        => start + (end - start) *
            (progress * progress * progress *
             (NumberExtension.Number<T>(6) * progress * progress
                 - NumberExtension.Number<T>(15) * progress + NumberExtension.Number<T>(10)
             )
            );

}