namespace MiHoMiao.Core.Reflection;

public interface INamedCollectable : ICollectable
{
    /// <summary>
    /// 可读性更高的一个名称.
    /// 收集时不会被使用. 仅用于前端显示.
    /// </summary>
    string ReadableName { get; }

}