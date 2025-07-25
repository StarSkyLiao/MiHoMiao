using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Core.Reflection;

public interface ICollectable
{
    /// <summary>
    /// 被收集时使用的名称.
    /// 需要保证使用唯一字符串.
    /// </summary>
    string UniqueName { get; }

    /// <summary>
    /// 被收集时使用的块.
    /// 例如: 设置为 [ ["1", "2", "3"] ] 时,
    /// 使用名称: "1"、"2.1"、"3.2.1" 都可以访问这个对象.
    /// 多个数组表示不同的访问方式, 例如设置不同的语言.
    /// </summary>
    string[][] NameGroups { get; }
    
    /// <summary>
    /// 收集所有继承自 ICollectable 接口的实例类
    /// </summary>
    [field: AllowNull, MaybeNull]
    public static Type[] Types => field ??=
    (
        from assembly in AppDomain.CurrentDomain.GetAssemblies()
        from type in assembly.GetTypes()
        where type.IsAssignableTo(typeof(ICollectable)) && !type.IsAbstract && !type.IsInterface
        select type
    ).ToArray();

}