using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Collections.Specialized;

namespace MiHoMiao.Jarfter.Runtime.Function.Template;

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
        using MutableString mutableString = new MutableString();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsAssignableTo(typeof(IJarfterFunc))) continue;
            if (type.IsAbstract || type.IsInterface) continue;
            IJarfterFunc? jarfterFunc = (IJarfterFunc?)Activator.CreateInstance(type);
            if (jarfterFunc is null) continue;
            mutableString.Clear();
            foreach (string name in jarfterFunc.JarfterFuncName)
            {
                if (mutableString.Length > 0) mutableString.Insert(0, '.');
                mutableString.Insert(0, name);
                if (ConflictNames.Contains(mutableString.Read())) continue;
                string readResult = mutableString.ToString();
                if (result.TryAdd(readResult, jarfterFunc)) continue;
                result.Remove(readResult);
                ConflictNames.Add(readResult);
            }
        }

        return result;
    }

    #endregion
    
}