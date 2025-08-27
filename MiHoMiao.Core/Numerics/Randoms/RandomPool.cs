namespace MiHoMiao.Core.Numerics.Randoms;

public class RandomPool<T>((T, double)[] entries)
{
    private readonly AliasPool<T> m_AliasPool = new AliasPool<T>(entries);
    private readonly List<(T item, double weight)> m_List = [];
    private bool m_VersionChanged;
    
    public void Add((T item, double weight) item)
    {
        if (item.weight == 0) return;
        m_VersionChanged = true;
        m_List.Add(item);
    }

    /// <summary>
    /// 根据权重，随机返回一个对象.
    /// </summary>
    public T? GetRandomly()
    {
        if (m_VersionChanged) m_AliasPool.Update(m_List);
        m_VersionChanged = false;
        return m_AliasPool.GetRandomly();
    }

    /// <summary>
    /// 根据权重，随机返回一个对象.
    /// </summary>
    public T GetRandomly(int seed) => m_AliasPool.GetRandomly(seed);

    public void Clear()
    {
        m_List.Clear();
        m_VersionChanged = true;
    }
    
}