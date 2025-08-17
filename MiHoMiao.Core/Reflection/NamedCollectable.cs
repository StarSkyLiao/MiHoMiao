using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MiHoMiao.Core.Reflection;

public abstract class NamedCollectable : ICollectable
{
    /// <summary>
    /// 可读性更高的一个中文名称.
    /// 收集时不会被使用. 仅用于前端显示.
    /// </summary>
    public abstract string ChineseName { get; }
    
    /// <summary>
    /// 可读性更高的一个英文名称.
    /// </summary>
    public abstract string EnglishName { get; }
    
    /// <summary>
    /// 该收集对象的包信息
    /// </summary>
    public PackageInfoAttribute? PackageInfo => field ??= GetType().Assembly.GetCustomAttribute<PackageInfoAttribute>();

    [field: AllowNull, MaybeNull]
    public virtual string UniqueName => field ??= $"{PackageInfo?.Name}::{EnglishName}";

    [field: AllowNull, MaybeNull]
    public virtual string[][] NameGroups => field ??=
    [
        [ChineseName, PackageInfo?.Name!],
        [EnglishName, PackageInfo?.Name!]
    ];

}