//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;
using System.Text;

namespace MiHoMiao.Core.Collections.Tool;

/// <summary>
/// 将集合的内容转化为字符串的工具
/// </summary>
public static class Viewer
{
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
    public static string GenericViewer<T>(this ICollection<T> collection, Func<T, string> toString, string left = "[", string right = "]", string split = ",")
        => ToString(collection, item => item switch
        {
            null => "null",
            ICollection clt => clt.CollectionViewer(left, right, split),
            ICollection<T> clt => clt.GenericViewer(left, right, split),
            _ => toString(item)
        }, left, right, split);

    private static string ToString<T>(ICollection<T> collection, Func<T, string> toString, string left, string right, string split)
    {
        if (collection.Count == 0) return $"{left}{right}";
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