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
        collection.UnionWith(items);
    }
    
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items) collection.Add(item);
    }
    
}