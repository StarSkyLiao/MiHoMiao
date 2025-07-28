//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// 表示一个具有固定容量、保持插入顺序的哈希集合。
/// 当集合达到容量上限时，移除最旧的元素以添加新元素。
/// 用于 LRU 缓存结构的设计.
/// </summary>
/// <typeparam name="T">集合中元素的类型，必须为非空类型。</typeparam>
public class LruHashSet<T>(int capacity) : ICollection<T> where T : notnull
{
    internal readonly LinkedList<T> LinkedList = [];
    private readonly Dictionary<T, LinkedListNode<T>> m_Dictionary = [];
    
    #region IEnumerableMethods
    
    /// <summary>
    /// 获取集合的枚举器，用于按插入顺序遍历元素。
    /// </summary>
    /// <returns>返回一个按插入顺序遍历元素的枚举器。</returns>
    public IEnumerator<T> GetEnumerator() => LinkedList.GetEnumerator();

    /// <summary>
    /// 获取非泛型枚举器，用于兼容非泛型接口。
    /// </summary>
    /// <returns>返回一个按插入顺序遍历元素的枚举器。</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
    
    #region ICollectionMembers

    /// <summary>
    /// 获取集合中元素的数量。
    /// </summary>
    public int Count => LinkedList.Count;

    /// <summary>
    /// 获取集合的版本号，每次修改集合时版本号递增。
    /// </summary>
    internal int Version { get; private set; }

    /// <summary>
    /// 获取一个值，指示集合是否为只读。
    /// </summary>
    bool ICollection<T>.IsReadOnly => false;
    
    /// <summary>
    /// 向集合中添加一个元素。
    /// 如果元素已存在，则将其移到集合开头。
    /// 如果集合已满，则移除最旧的元素以添加新元素。
    /// </summary>
    /// <param name="item">要添加的元素。</param>
    public void Add(T item)
    {
        ++Version;
        if (m_Dictionary.TryGetValue(item, out LinkedListNode<T>? node))
        {
            LinkedList.Remove(node);
            LinkedList.AddFirst(node);
        }
        else
        {
            node = LinkedList.AddFirst(item);
            m_Dictionary.Add(item, node);
            while (LinkedList.Count > capacity)
            {
                node = LinkedList.Last;
                m_Dictionary.Remove(node!.Value);
                LinkedList.RemoveLast();
            }
        }
    }

    /// <summary>
    /// 清空集合中的所有元素。
    /// </summary>
    public void Clear()
    {
        ++Version;
        LinkedList.Clear();
        m_Dictionary.Clear();
    }
    
    /// <summary>
    /// 检查集合是否包含指定元素。
    /// </summary>
    /// <param name="item">要检查的元素。</param>
    /// <returns>如果集合包含该元素，则返回 true；否则返回 false。</returns>
    public bool Contains(T item) => m_Dictionary.ContainsKey(item);
    
    /// <summary>
    /// 将集合中的元素复制到指定数组中。
    /// </summary>
    /// <param name="array">目标数组。</param>
    public void CopyTo(T[] array) => CopyTo(array, 0);
    
    /// <summary>
    /// 将集合中的元素复制到指定数组的指定索引处。
    /// </summary>
    /// <param name="array">目标数组。</param>
    /// <param name="arrayIndex">目标数组的起始索引。</param>
    public void CopyTo(T[] array, int arrayIndex) => LinkedList.CopyTo(array, arrayIndex);

    /// <summary>
    /// 从集合中移除指定元素。
    /// </summary>
    /// <param name="item">要移除的元素。</param>
    /// <returns>如果成功移除元素，则返回 true；否则返回 false。</returns>
    public bool Remove(T item)
    {
        if (!m_Dictionary.TryGetValue(item, out LinkedListNode<T>? node)) return false;
        ++Version;
        LinkedList.Remove(node);
        return m_Dictionary.Remove(item);
    }

    #endregion
    
}