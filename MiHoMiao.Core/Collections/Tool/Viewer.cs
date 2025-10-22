//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;
using System.Text;
using MiHoMiao.Core.Collections.Unsafe;

namespace MiHoMiao.Core.Collections.Tool;

/// <summary>
/// 将集合的内容转化为字符串的工具
/// </summary>
public static class Viewer
{
    /// <summary>
    /// 将任意泛型集合 IEnumerable 格式化为易读的字符串。
    /// </summary>
    /// <typeparam name="T">集合元素类型，必须是非可空引用类型或值类型。</typeparam>
    /// <param name="enumerable">要格式化的集合。</param>
    /// <param name="left">整体左边界符，默认“[”。</param>
    /// <param name="right">整体右边界符，默认“]”。</param>
    /// <param name="split">元素之间的分隔符，默认“,”。</param>
    /// <param name="toString">可选的自定义元素转字符串委托；为 null 时使用元素自身的 ToString()。</param>
    /// <returns>格式化后的字符串。</returns>
    /// <exception cref="ArgumentNullException">当 enumerable 为 null 时抛出。</exception>
    public static string EnumerableViewer<T>(this IEnumerable<T> enumerable,
        string left = "[", string right = "]", string split = ",", Func<T, string>? toString = null
    ) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(enumerable);

        using InterpolatedString interpolated = new InterpolatedString(64);
        interpolated.Append(left);

        bool isFirst = true;
        foreach (T item in enumerable)
        {
            if (!isFirst) interpolated.Append(split);
            interpolated.Append(toString != null ? toString(item) : item.ToString() ?? string.Empty);
            isFirst = false;
        }

        interpolated.Append(right);
        return interpolated.ToString();
    }



    /// <summary>
    /// 将非泛型集合的内容输出为字符串
    /// </summary>
    public static string CollectionViewer(this ICollection collection, string left = "[", string right = "]", string split = ",")
        => ToString(collection, item => item switch
        {
            null => "null",
            ICollection clt => clt.CollectionViewer(left, right, split),
            _ => item.ToString()!
        }, left, right, split);

    /// <summary>
    /// 将泛型集合的内容输出为字符串
    /// </summary>
    public static string GenericViewer<T>(this IEnumerable<T> enumerable, string left = "[", string right = "]", string split = ",")
        => enumerable.ToArray().GenericViewer(left, right, split);

    /// <summary>
    /// 将泛型集合的内容输出为字符串
    /// </summary>
    public static string GenericViewer<T>(this ICollection<T> collection, string left = "[", string right = "]", string split = ",")
        => ToString(collection, item => item switch
        {
            null => "null",
            ICollection clt => clt.CollectionViewer(left, right, split),
            ICollection<T> clt => clt.GenericViewer(left, right, split),
            _ => item.ToString()!
        }, left, right, split);

    /// <summary>
    /// 将泛型集合的内容输出为字符串（带自定义转换）
    /// </summary>
    public static string GenericViewer<T>(this IEnumerable<T> collection, Func<T, string> toString, string left = "[", string right = "]", string split = ",")
        => ToString(collection, item => item switch
        {
            null => "null",
            ICollection clt => clt.CollectionViewer(left, right, split),
            ICollection<T> clt => clt.GenericViewer(left, right, split),
            _ => toString(item)
        }, left, right, split);

    private static string ToString<T>(IEnumerable<T> collection, Func<T, string> toString, string left, string right, string split)
    {
        if (!collection.Any()) return $"{left}{right}";
        StringBuilder sb = new StringBuilder().Append(left);
        foreach (T item in collection)
            sb.Append(toString(item)).Append(split);
        if (split.Length > 0) sb.Length -= split.Length;
        return sb.Append(right).ToString();
    }

    private static string ToString(ICollection collection, Func<object, string> toString, string left, string right, string split)
    {
        if (collection.Count == 0) return $"{left}{right}";
        StringBuilder sb = new StringBuilder().Append(left);
        foreach (object? item in collection)
            sb.Append(toString(item)).Append(split);
        if (split.Length > 0) sb.Length -= split.Length;
        return sb.Append(right).ToString();
    }
}