using System.Reflection;

namespace MiHoMiao.Migxn.Reflection;


internal class ReflectTool
{
    
    /// <summary>
    /// 根据类型名称获取指定的类型
    /// </summary>
    public static Type? LoadType(string typeName) => ParseCoreType(typeName) ?? Type.GetType(typeName) ?? Type.GetType(ParseTypeName(typeName)) ?? null;

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

    #region TypeMap
    
    private static readonly Dictionary<(Type, Type), Type> s_IntLogicTypeMap = new Dictionary<(Type, Type), Type>
    {
        [(typeof(bool), typeof(bool))] = typeof(bool),
            
        [(typeof(char), typeof(char))] = typeof(char),
        [(typeof(char), typeof(int))] = typeof(int),
        [(typeof(char), typeof(long))] = typeof(long),
    
        [(typeof(int), typeof(char))] = typeof(int),
        [(typeof(int), typeof(int))] = typeof(int),
        [(typeof(int), typeof(long))] = typeof(long),
            
        [(typeof(long), typeof(char))] = typeof(long),
        [(typeof(long), typeof(int))] = typeof(long),
        [(typeof(long), typeof(long))] = typeof(long),
    };
    
    private static readonly Dictionary<(Type, Type), Type> s_IntShiftTypeMap = new Dictionary<(Type, Type), Type>
    {
        [(typeof(char), typeof(char))] = typeof(int),
        [(typeof(char), typeof(int))] = typeof(int),
        [(typeof(char), typeof(long))] = typeof(int),

        [(typeof(int), typeof(char))] = typeof(int),
        [(typeof(int), typeof(int))] = typeof(int),
        [(typeof(int), typeof(long))] = typeof(int),
        
        [(typeof(long), typeof(char))] = typeof(long),
        [(typeof(long), typeof(int))] = typeof(long),
        [(typeof(long), typeof(long))] = typeof(long),
    };
    
    private static readonly Dictionary<(Type, Type), Type> s_CalculationTypeMap = new Dictionary<(Type, Type), Type>
    {
        [(typeof(char), typeof(char))] = typeof(char),
        [(typeof(char), typeof(int))] = typeof(int),
        [(typeof(char), typeof(long))] = typeof(long),
        [(typeof(char), typeof(float))] = typeof(float),
        [(typeof(char), typeof(double))] = typeof(double),

        [(typeof(int), typeof(char))] = typeof(int),
        [(typeof(int), typeof(int))] = typeof(int),
        [(typeof(int), typeof(long))] = typeof(long),
        [(typeof(int), typeof(float))] = typeof(float),
        [(typeof(int), typeof(double))] = typeof(double),
        
        [(typeof(long), typeof(char))] = typeof(long),
        [(typeof(long), typeof(int))] = typeof(long),
        [(typeof(long), typeof(long))] = typeof(long),
        [(typeof(long), typeof(float))] = typeof(float),
        [(typeof(long), typeof(double))] = typeof(double),
        
        [(typeof(float), typeof(char))] = typeof(float),
        [(typeof(float), typeof(int))] = typeof(float),
        [(typeof(float), typeof(long))] = typeof(float),
        [(typeof(float), typeof(float))] = typeof(float),
        [(typeof(float), typeof(double))] = typeof(double),
        
        [(typeof(double), typeof(char))] = typeof(double),
        [(typeof(double), typeof(int))] = typeof(double),
        [(typeof(double), typeof(long))] = typeof(double),
        [(typeof(double), typeof(float))] = typeof(double),
        [(typeof(double), typeof(double))] = typeof(double),
    };

    #endregion
    
    /// <summary>
    /// 返回两个整形逻辑运算的结果类型
    /// </summary>
    public static Type? IntLogicType(Type left, Type right) => s_IntLogicTypeMap.GetValueOrDefault((left, right));

    /// <summary>
    /// 返回两个整形位移运算的结果类型
    /// </summary>
    public static Type? IntShiftType(Type left, Type right) => s_IntShiftTypeMap.GetValueOrDefault((left, right));
    
    /// <summary>
    /// 返回两个整形位移运算的结果类型
    /// </summary>
    public static Type? CalculationType(Type left, Type right) => s_CalculationTypeMap.GetValueOrDefault((left, right));
    
    public static Delegate? LoadMethod(string methodFullName, IList<string>? parameterTypeNames = null)
    {
        // Split the input string into type and method names
        string[] parts = methodFullName.Split('.');
        if (parts.Length < 2) return null;

        // Construct the full type name (excluding the last part which is the method name)
        string typeName = string.Join(".", parts.Take(parts.Length - 1));
        string methodName = parts[^1];
        
        // Get the type from the type name
        Type? type = Type.GetType(typeName) ?? Type.GetType(ParseMethodName(typeName));
        if (type == null) return null;

        // Find all methods with the specified name (including public, non-public, static, and instance methods)
        MethodInfo[] methods = type.GetMethods(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static
        ).Where(m => m.Name == methodName).ToArray();

        if (methods.Length == 0) return null;

        // If parameterTypeNames is provided, find the best matching method
        MethodInfo? methodInfo = null;
        if (parameterTypeNames is { Count: > 0 })
        {
            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();
                // Check if the method's parameter list is compatible with parameterTypeNames
                if (!IsParameterMatch(parameters, parameterTypeNames)) continue;
                methodInfo = method;
                break;
            }
        }
        else
        {
            // If no parameterTypeNames provided, take the first method (original behavior)
            methodInfo = methods[0];
        }

        if (methodInfo == null) return null;

        // Get the method's signature (return type and parameters)
        Type returnType = methodInfo.ReturnType;
        Type[] parameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();

        // Create a delegate type dynamically based on the method signature
        Type delegateType =
            // For methods with no return value, use Action or Action<...>
            returnType == typeof(void) ? GetActionType(parameterTypes) :
            // For methods with a return value, use Func<..., TReturn>
            GetFuncType(parameterTypes, returnType);
        
        return methodInfo.CreateDelegate(delegateType);
    }


    
    
    private static string ParseMethodName(string methodName)
    {
        if (string.IsNullOrEmpty(methodName)) return methodName;

        // Check if the string starts with [AssemblyName]
        if (!methodName.StartsWith('%')) return methodName;
        
        int endBracket = methodName.LastIndexOf('%');
        if (endBracket <= 1 || endBracket >= methodName.Length - 1) return methodName;
        
        // Extract assembly name (between %)
        string assemblyName = methodName.Substring(1, endBracket - 1);
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.First(assembly => assembly.GetName().Name == assemblyName);

        return $"{methodName[(endBracket + 1)..]}, {assembly.GetName().FullName}";
    }
    
    private static bool IsParameterMatch(ParameterInfo[] parameters, IList<string> parameterTypeNames)
    {
        // If parameterTypeNames is longer than the method's parameter list, it's not a match
        if (parameterTypeNames.Count > parameters.Length) return false;
        // Compare each parameter's type name with the provided type names
        return !parameterTypeNames.Where((t, i) => parameters[i].ParameterType.Name != t).Any();
    }
    
    private static Type GetActionType(Type[] parameterTypes)
    {
        return parameterTypes.Length switch
        {
            0 => typeof(Action),
            1 => typeof(Action<>).MakeGenericType(parameterTypes),
            2 => typeof(Action<,>).MakeGenericType(parameterTypes),
            3 => typeof(Action<,,>).MakeGenericType(parameterTypes),
            4 => typeof(Action<,,,>).MakeGenericType(parameterTypes),
            5 => typeof(Action<,,,,>).MakeGenericType(parameterTypes),
            6 => typeof(Action<,,,,,>).MakeGenericType(parameterTypes),
            7 => typeof(Action<,,,,,,>).MakeGenericType(parameterTypes),
            8 => typeof(Action<,,,,,,,>).MakeGenericType(parameterTypes),
            _ => throw new NotSupportedException("Methods with more than 8 parameters are not supported.")
        };
    }

    private static Type GetFuncType(Type[] parameterTypes, Type returnType)
    {
        Type[] genericArgs = parameterTypes.Concat([returnType]).ToArray();
        return parameterTypes.Length switch
        {
            0 => typeof(Func<>).MakeGenericType(returnType),
            1 => typeof(Func<,>).MakeGenericType(genericArgs),
            2 => typeof(Func<,,>).MakeGenericType(genericArgs),
            3 => typeof(Func<,,,>).MakeGenericType(genericArgs),
            4 => typeof(Func<,,,,>).MakeGenericType(genericArgs),
            5 => typeof(Func<,,,,,>).MakeGenericType(genericArgs),
            6 => typeof(Func<,,,,,,>).MakeGenericType(genericArgs),
            7 => typeof(Func<,,,,,,,>).MakeGenericType(genericArgs),
            8 => typeof(Func<,,,,,,,,>).MakeGenericType(genericArgs),
            _ => throw new NotSupportedException("Methods with more than 8 parameters are not supported.")
        };
    }
    
}