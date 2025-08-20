// namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
//
// internal class BinaryTypeChecker
// {
//     public static Type? CanAdd(Type type1, Type type2)
//     {
//         // 检查常见内置类型的加法及其返回类型
//         Type? builtInResult = GetBuiltInAdditionResultType(type1, type2);
//         // 检查用户自定义的加法运算符及其返回类型
//         return builtInResult ?? GetAdditionOperatorResultType(type1, type2);
//     }
//
//     // 检查内置类型加法并返回结果类型
//     private static Type? GetBuiltInAdditionResultType(Type type1, Type type2)
//     {
//         // 字符串连接
//         if (type1 == typeof(string) || type2 == typeof(string))
//         {
//             if (IsBuiltInNumericType(type1) || IsBuiltInNumericType(type2) || type1 == typeof(string) || type2 == typeof(string))
//             {
//                 return typeof(string); // 字符串连接返回 string
//             }
//         }
//
//         // 数值类型加法
//         if (IsBuiltInNumericType(type1) && IsBuiltInNumericType(type2))
//         {
//             // 确定返回类型（基于类型提升规则）
//             if (type1 == typeof(decimal) || type2 == typeof(decimal))
//                 return typeof(decimal);
//             if (type1 == typeof(double) || type2 == typeof(double))
//                 return typeof(double);
//             if (type1 == typeof(float) || type2 == typeof(float))
//                 return typeof(float);
//             if (type1 == typeof(ulong) || type2 == typeof(ulong))
//                 return typeof(ulong);
//             if (type1 == typeof(long) || type2 == typeof(long))
//                 return typeof(long);
//             if (type1 == typeof(uint) || type2 == typeof(uint))
//                 return typeof(uint);
//             return typeof(int); // 默认返回 int（例如 byte + byte）
//         }
//
//         return null;
//     }
//
//     // 检查是否为内置数值类型
//     private static bool IsBuiltInNumericType(Type type)
//     {
//         return type == typeof(int) || type == typeof(double) || type == typeof(float) ||
//                type == typeof(decimal) || type == typeof(long) || type == typeof(short) ||
//                type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) ||
//                type == typeof(byte) || type == typeof(sbyte);
//     }
//
//     // 检查用户自定义加法运算符并返回结果类型
//     private static Type? GetAdditionOperatorResultType(Type type1, Type type2)
//     {
//         // 检查 type1 是否定义了 operator +
//         MethodInfo[] methods = type1.GetMethods(BindingFlags.Public | BindingFlags.Static);
//         foreach (var method in methods)
//         {
//             if (method.Name == "op_Addition")
//             {
//                 var parameters = method.GetParameters();
//                 if (parameters.Length == 2 &&
//                     parameters[0].ParameterType == type1 &&
//                     parameters[1].ParameterType == type2)
//                 {
//                     return method.ReturnType; // 返回 operator + 的返回类型
//                 }
//             }
//         }
//
//         // 检查 type2 是否定义了 operator +
//         methods = type2.GetMethods(BindingFlags.Public | BindingFlags.Static);
//         foreach (var method in methods)
//         {
//             if (method.Name == "op_Addition")
//             {
//                 var parameters = method.GetParameters();
//                 if (parameters.Length == 2 &&
//                     parameters[0].ParameterType == type1 &&
//                     parameters[1].ParameterType == type2)
//                 {
//                     return method.ReturnType; // 返回 operator + 的返回类型
//                 }
//             }
//         }
//
//         return null; // 未找到加法运算符
//     }
// }