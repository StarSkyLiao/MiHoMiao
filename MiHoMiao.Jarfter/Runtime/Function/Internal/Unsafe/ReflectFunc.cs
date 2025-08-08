using System.Reflection;
using JetBrains.Annotations;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;
using MiHoMiao.Jarfter.Runtime.Function.Internal.Call;
using MiHoMiao.Jarfter.Runtime.Function.Template;

namespace MiHoMiao.Jarfter.Runtime.Function.Internal.Unsafe;

[UsedImplicitly]
public class ReflectFunc : IJarfterFunc
{
    public string[] JarfterFuncName { get; } = ["unsafe.reflect", "internal"];

    public void RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input)
    {
        JarfterFuncImpl(jarfterContext,
            IJarfterFunc.JarfterParse<string>(jarfterContext, input),
            IJarfterFunc.JarfterParse<string>(jarfterContext, input),
            jarfterContext.ParsingIndex < input.Length
                ? IJarfterFunc.JarfterParse<JarfterArray<string>>(jarfterContext, input)
                : null
        );
    }
    
    public static void JarfterFuncImpl(JarfterContext jarfterContext, string arg0, string arg1, JarfterArray<string>? arg2)
    {
        Delegate? method = LoadMethod(arg1, arg2?.Content);
        if (method is null) throw new NotSupportedException($"Method {arg1} is not found!");
        CallHelper.Register(arg0, method);
    }
    
    public static Delegate? LoadMethod(string methodFullName, IList<string>? parameterTypeNames = null)
    {
        // Split the input string into type and method names
        string[] parts = methodFullName.Split('.');
        if (parts.Length < 2) return null;

        // Construct the full type name (excluding the last part which is the method name)
        string typeName = string.Join(".", parts.Take(parts.Length - 1));
        string methodName = parts[^1];

        // Get the type from the type name
        Type? type = Type.GetType(typeName);
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