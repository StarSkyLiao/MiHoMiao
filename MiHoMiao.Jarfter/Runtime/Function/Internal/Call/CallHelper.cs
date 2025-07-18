using System.Collections.Concurrent;
using System.Reflection;
using MiHoMiao.Jarfter.Core;
using MiHoMiao.Jarfter.Core.Func;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Call;

public static class CallHelper
{
    // ---------------- 1. 注册表 ----------------
    private static readonly ConcurrentDictionary<string, Delegate> m_Map = [];

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
        
        m_Map[alias] = method;
    }

    // ---------------- 2. 统一调用 ----------------
    /// <summary>
    /// 调用已注册的 alias 方法。
    /// 方法的参数为 args.
    /// </summary>
    public static object? Invoke(JarfterContext jarfterContext, string alias, params ReadOnlySpan<string> args)
    {
        if (!m_Map.TryGetValue(alias, out var method))
            throw new InvalidOperationException($"未注册的方法别名：{alias}");

        ParameterInfo[] parameterInfos = method.Method.GetParameters();
        if (parameterInfos.Length != args.Length)
            throw new ArgumentException($"参数个数不符：期望 {parameterInfos.Length}，实际 {args.Length}");

        var parsedArgs = new object?[parameterInfos.Length];
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
        if (m_ParseMap.ContainsKey(type)) return true;
        return type.GetInterface("System.ISpanParsable`1") != null;
    }
    
    private static readonly Dictionary<Type, MethodInfo> m_ParseMap = [];

    internal static MethodInfo? LoadParser(Type type)
    {
        if (m_ParseMap.TryGetValue(type, out MethodInfo? parser)) return parser;
        if (type == typeof(string))
        {
            parser = ((Func<JarfterContext, string, string>)StringParser).Method;
        }
        else
        {
            parser = typeof(IJarfterFunc).GetMethod(
                nameof(IJarfterFunc.JarfterParse), BindingFlags.Public | BindingFlags.Static,
                [typeof(JarfterContext), typeof(string), typeof(IFormatProvider)]
            )?.MakeGenericMethod(type);
        }
        if (parser == null) return null;
        return m_ParseMap[type] = parser;
    }
        
    internal static T InvokeParser<T>(JarfterContext jarfterContext, string input) 
        => (T)InvokeParser(typeof(T), jarfterContext, input, null);

    internal static T InvokeParser<T>(JarfterContext jarfterContext, string input, IFormatProvider? provider)
        => (T)InvokeParser(typeof(T), jarfterContext, input, provider);
    
    internal static object InvokeParser(Type type, JarfterContext jarfterContext, string input) 
        => InvokeParser(type, jarfterContext, input, null);
    
    internal static object InvokeParser(Type type, JarfterContext jarfterContext, string input, IFormatProvider? provider)
    {
        MethodInfo? parser = LoadParser(type);
        if (parser == null) throw new ArgumentException($"Type {type.FullName} has no parser method.");
        int paramCount = parser.GetParameters().Length;
        return parser.Invoke(null, (paramCount is 3) ? [jarfterContext, input, provider] : [jarfterContext, input])!;
    }

    private static string StringParser(JarfterContext jarfterContext, string input) 
        => IJarfterFunc.JarfterParse<string>(jarfterContext, input);
    
}