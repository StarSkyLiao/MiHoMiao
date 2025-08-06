using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



// 示例类型
Type intType = typeof(int);
Type stringType = typeof(string);
Type customType = typeof(MyCustomType);

// 检查是否实现了 ISpanParsable<TSelf>
bool intImplements = ImplementsISpanParsable(intType); // true（int 实现了 ISpanParsable<int>）
bool stringImplements = ImplementsISpanParsable(stringType); // false
bool customImplements = ImplementsISpanParsable(customType); // 取决于 MyCustomType 是否实现

Console.WriteLine($"int: {intImplements}");
Console.WriteLine($"string: {stringImplements}");
Console.WriteLine($"custom: {customImplements}");

static bool ImplementsISpanParsable(Type type)
{
    // 检查类型是否实现了 ISpanParsable<TSelf> 接口
    return type.GetInterfaces()
        .Any(i => i.IsGenericType && 
                  i.GetGenericTypeDefinition() == typeof(ISpanParsable<>));
}


struct MyCustomType;