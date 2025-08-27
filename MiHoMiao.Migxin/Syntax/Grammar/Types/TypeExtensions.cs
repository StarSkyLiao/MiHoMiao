using System.Diagnostics;
using System.Reflection;

namespace MiHoMiao.Migxin.Syntax.Grammar.Types;

internal class TypeExtensions
{
    /// <summary>
    /// 查找两个类型的公共父类型
    /// </summary>
    public static Type FindCommonBaseType(Type type1, Type type2)
    {
        // 如果两个类型相同，直接返回
        if (type1 == type2) return type1;
        if (s_CommonBaseType.TryGetValue((type1, type2), out Type? result)
            || s_CommonBaseType.TryGetValue((type2, type1), out result)
           ) return result;

        // 获取两个类型的继承链
        IEnumerable<Type> type1Hierarchy = GetTypeHierarchy(type1);
        IEnumerable<Type> type2Hierarchy = GetTypeHierarchy(type2);

        // 查找共同的父类
        result = type1Hierarchy.Intersect(type2Hierarchy).FirstOrDefault();
        Debug.Assert(result != null);
        s_CommonBaseType[(type1, type2)] = result;
        return result;
        static IEnumerable<Type> GetTypeHierarchy(Type? type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }

    #region FindCommonBaseType

    private static readonly Dictionary<(Type, Type), Type> s_CommonBaseType = [];

    #endregion

    /// <summary>
    /// 根据类型名称获取指定的类型
    /// 类型名称可以是:
    /// BaseType: i32, r64
    /// CoreType: System.Int32, System.Single
    /// FullName: [System.Console]System.Console
    /// </summary>
    public static Type? LoadType(string typeName) => ParseCoreType(typeName) ?? Type.GetType(typeName) ?? Type.GetType(ParseTypeName(typeName)) ?? null;

    #region LoadType

    private static readonly Dictionary<string, Assembly> s_Assemblies = [];
    
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