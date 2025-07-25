using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Core.Collections.Unsafe;

// ReSharper disable StaticMemberInGenericType

namespace MiHoMiao.Core.Reflection;

/// <summary>
/// A static class that is able to collect child Classes that implement interface T.
/// You may ensure that every non_abstract type that implement interface T should implement .ctor().
/// </summary>
public static class Collect<T> where T : ICollectable
{
    [field: AllowNull, MaybeNull] 
    private static Dictionary<string, T> CollectedTypes { get; }

    [field: AllowNull, MaybeNull] 
    public static Dictionary<string, T> Datas { get; } = [];
    
    public static ImmutableArray<T> DataArray { get; private set; }
    
    private static HashSet<string> ConflictNames { get; } = [];

    static Collect()
    {
        CollectedTypes = new Dictionary<string, T>(
            from type in ICollectable.Types
            where typeof(T).IsAssignableFrom(type)
            let instance = (T)typeof(Singleton<>).MakeGenericType(type)!
                .GetProperty(nameof(Singleton<>.Instance))!.GetValue(null)
            where instance != null
            select new KeyValuePair<string, T>(instance.UniqueName ?? throw new NullReferenceException(instance.GetType().FullName), instance)
        );
            
        using MutableString dynamicString = new MutableString(16);
        HashSet<T> spaceTimeDatas = [];
        foreach (KeyValuePair<string, T> item in CollectedTypes)
        {
            T data = item.Value;
            spaceTimeDatas.Add(item.Value);
            foreach (string[] nameGroup in data.NameGroups)
            {
                dynamicString.Clear();
                foreach (string name in nameGroup)
                {
                    if (dynamicString.Length > 0) dynamicString.Insert(0, '.');
                    dynamicString.Insert(0, name);
                    if (ConflictNames.Contains(dynamicString.Read())) continue;
                    string readResult = dynamicString.ToString();
                    if (Datas.TryAdd(readResult, data)) continue;
                    Datas.Remove(readResult);
                    ConflictNames.Add(readResult);
                }
            }
        }
        DataArray = [..spaceTimeDatas];
    }
    
}