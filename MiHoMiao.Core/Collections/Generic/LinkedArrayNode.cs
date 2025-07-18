namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// 注意: 指定元素的索引值并不固定, 可能会发生变化以节省内存空间.
/// 因此, 该节点的 Version 信息与链式数组的 Version 信息不匹配时会发生错误.
/// </summary>
public readonly struct LinkedArrayNode<T>
{
    public readonly int Index;
    internal readonly int Version;
    internal readonly LinkedArray<T> Target;

    public T Value
    {
        get
        {
            if (Target.Version != Version) throw new InvalidOperationException("VersionNotMatch");
            return Target.Items[Index].Value;
        }
    }

    internal LinkedArrayNode(int index, LinkedArray<T> target)
    {
        Index = index;
        Target = target;
        Version = target.Version;
    }
}