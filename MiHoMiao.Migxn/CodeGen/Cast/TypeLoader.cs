using System.Reflection;

namespace MiHoMiao.Migxn.CodeGen.Cast;

public static class TypeLoader
{
    /// <summary>
    /// 根据类型名称获取指定的类型
    /// </summary>
    public static Type LoadType(string typeName) => ParseCoreType(typeName) ?? Type.GetType(typeName) ?? Type.GetType(ParseTypeName(typeName)) ?? typeof(void);

    #region LoadType

    private static readonly Dictionary<string, Type> s_CoreType = new Dictionary<string, Type>
    {
        ["char"] = typeof(char),
        ["i32"] = typeof(int),
        ["i64"] = typeof(long),
        ["r32"] = typeof(float),
        ["r64"] = typeof(double),
        ["bool"] = typeof(bool),
        ["string"] = typeof(string),
    };
    
    private static Type? ParseCoreType(string typeName) => s_CoreType.GetValueOrDefault(typeName);
    
    private static readonly Dictionary<string, Assembly> s_Assemblies = [];
    
    private static string ParseTypeName(string typeName)
    {
        if (string.IsNullOrEmpty(typeName)) return typeName;

        // Check if the string starts with [AssemblyName]
        if (!typeName.StartsWith('[')) return typeName;
        
        int endBracket = typeName.LastIndexOf(']');
        if (endBracket <= 1 || endBracket >= typeName.Length - 1) return typeName;
        
        // Extract assembly name (between %)
        string assemblyName = typeName.Substring(1, endBracket - 1);
        if (!s_Assemblies.TryGetValue(assemblyName, out Assembly? assembly)) return typeName;
        
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly item in assemblies) s_Assemblies[item.GetName().Name!] = item;
        if (!s_Assemblies.TryGetValue(assemblyName, out assembly)) return typeName;

        return $"{typeName[(endBracket + 1)..]}, {assembly.GetName().FullName}";
    }

    #endregion

}