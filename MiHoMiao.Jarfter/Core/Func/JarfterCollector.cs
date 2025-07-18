using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Specialized;

namespace MiHoMiao.Jarfter.Core.Func;

public static class JarfterCollector
{

    public static HashSet<string> ConflictNames { get; } = [];

    [field: AllowNull, MaybeNull]
    public static Dictionary<string, IJarfterFunc> JarfterFuncTable => field ??= JarfterFuncInitiation();

    public static IJarfterFunc? LoadFunc(string funcName) => JarfterFuncTable.GetValueOrDefault(funcName);
    
    #region Helper

    private static Dictionary<string, IJarfterFunc> JarfterFuncInitiation()
    {
        Dictionary<string, IJarfterFunc> result = [];
        using DynamicString dynamicString = new DynamicString();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsAssignableTo(typeof(IJarfterFunc))) continue;
            if (type.IsAbstract || type.IsInterface) continue;
            IJarfterFunc? jarfterFunc = (IJarfterFunc?)Activator.CreateInstance(type);
            if (jarfterFunc is null) continue;
            dynamicString.Clear();
            foreach (string name in jarfterFunc.JarfterFuncName)
            {
                if (dynamicString.Length > 0) dynamicString.Insert(0, '.');
                dynamicString.Insert(0, name);
                if (ConflictNames.Contains(dynamicString.Read())) continue;
                string readResult = dynamicString.ToString();
                if (result.TryAdd(readResult, jarfterFunc)) continue;
                result.Remove(readResult);
                ConflictNames.Add(readResult);
            }
        }

        return result;
    }

    #endregion
    
}