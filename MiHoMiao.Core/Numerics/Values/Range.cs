using System.Numerics;

namespace MiHoMiao.Core.Numerics.Values;

/// <summary>
/// 表示一个从 Start 到 End 之间的范围.
/// 这个范围两端都是闭区间.
/// </summary>
public readonly record struct Range<T>(T Start, T End) where T : INumberBase<T>
{
    
    private static readonly Comparer<T> s_Comparer = Comparer<T>.Default;
    
    public bool Contains(T value) => s_Comparer.Compare(Start, value) <= 0 && s_Comparer.Compare(value, End) <= 0;

}