using System.Collections.Concurrent;
using System.Reflection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Call;

public static class CallHelper
{
    // ---------------- 1. 注册表 ----------------
    private static readonly ConcurrentDictionary<string, Delegate> s_Map = [];

    /// <summary>
    /// 注册一个别名为 alias 的静态方法，供后续 Invoke 使用。
    /// 重复注册同名方法时, 会直接覆盖原先的方法.
    /// </summary>
    /// <exception cref="ArgumentException">方法签名不符合要求</exception>
    public static void Register(string alias, Delegate method)
    {
        ArgumentNullException.ThrowIfNull(method);
        
        foreach (ParameterInfo p in method.Method.GetParameters())
        {
            if (IsParseable(p.ParameterType)) continue;
            throw new ArgumentException($"参数类型 {p.ParameterType} 未实现 IParse<> 或不是 string");
        }
        
        s_Map[alias] = method;
    }

    // ---------------- 2. 统一调用 ----------------
    /// <summary>
    /// 调用已注册的 alias 方法。
    /// 方法的参数为 args.
    /// </summary>
    public static object? Invoke(JarfterContext jarfterContext, string alias, params IList<string> args)
    {
        if (!s_Map.TryGetValue(alias, out Delegate? method)) throw new InvalidOperationException($"未注册的方法别名：{alias}");

        ParameterInfo[] parameterInfos = method.Method.GetParameters();
        if (args.Count < parameterInfos.Length)
            throw new ArgumentException($"参数个数不符：期望 {parameterInfos.Length}，实际 {args.Count}");

        object?[] parsedArgs = new object?[parameterInfos.Length];
        for (int i = 0; i < parameterInfos.Length; i++)
        {
            Type targetType = parameterInfos[i].ParameterType;
            try
            {
                parsedArgs[i] = InvokeParser(targetType, jarfterContext, args[i]);
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException($"第 {i} 个参数解析失败：{ex.Message}", ex);
            }
        }

        return method.DynamicInvoke(parsedArgs);
    }

    // ---------------- 3. 工具函数 ----------------
    /// <summary>
    /// 判断一个类型是否实现 ISpanParsable;
    /// </summary>
    private static bool IsParseable(Type type)
    {
        if (s_ParseMap.ContainsKey(type)) return true;
        // 检查类型是否实现了 ISpanParsable<TSelf> 接口
        return type.GetInterfaces().Any(item
            => item.IsGenericType &&
               item.GetGenericTypeDefinition() == typeof(ISpanParsable<>)
        );
    }
    
    private static readonly Dictionary<Type, MethodInfo> s_ParseMap = [];

    private static MethodInfo? LoadParser(Type type)
    {
        if (s_ParseMap.TryGetValue(type, out MethodInfo? parser)) return parser;
        MethodInfo? methodInfo = typeof(IJarfterFunc).GetMethod(
            nameof(IJarfterFunc.JarfterParseFromString), BindingFlags.NonPublic | BindingFlags.Static,
            [typeof(JarfterContext), typeof(string)]
        );
        parser = methodInfo?.MakeGenericMethod(type);
        if (parser == null) return null;
        return s_ParseMap[type] = parser;
    }
        
    internal static T InvokeParser<T>(JarfterContext jarfterContext, string input) 
        => (T)InvokeParser(typeof(T), jarfterContext, input);
    
    internal static object InvokeParser(Type type, JarfterContext jarfterContext, string input)
    {
        MethodInfo? parser = LoadParser(type);
        if (parser == null) throw new ArgumentException($"Type {type.FullName} has no parser method.");
        return parser.Invoke(null, [jarfterContext, input])!;
    }
    
}