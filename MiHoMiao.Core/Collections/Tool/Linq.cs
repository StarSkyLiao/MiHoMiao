namespace MiHoMiao.Core.Collections.Tool;

public static class Linq
{
    public static void RemoveWhere<T>(this List<T> collection, Predicate<T> predicate)
    {
        collection.RemoveAll(predicate);
    }
    
    public static void RemoveWhere<T>(this ICollection<T> collection, Predicate<T> predicate)
    {
        List<T> toBeRemoved = [];
        foreach (T item in collection) if (predicate(item)) toBeRemoved.Add(item);
        foreach (T item in toBeRemoved) collection.Remove(item);
    }
    
    public static void AddRange<T>(this HashSet<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items) collection.Add(item);
    }
    
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items) collection.Add(item);
    }

    /// <summary>
    /// 确保序列至少具有 count 个元素
    /// </summary>
    public static IEnumerable<T?> EnsureLength<T>(this IEnumerable<T> enumerable, int count, T? @default = default)
    {
        int index = 0;
        foreach (T item in enumerable)
        {
            yield return item;
            ++index;
        }

        while (index < count)
        {
            yield return @default;
            ++index;
        }
    }
    
}