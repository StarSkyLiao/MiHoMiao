//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;
using System.Diagnostics;

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// 可重复元素跳表的一个实现.
/// 跳表是一个有序的表, 同时可以通过索引快速地获取元素值.
/// 其增删改查等操作的平均时间复杂度都是 O(log(n)).
/// </summary>
public class SkipList<T> : IList<T>, ICollection where T : IComparable<T>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Random m_Random = new Random();
    private static readonly SkipListNode[] s_UpdateArray = new SkipListNode[MaxLevel];

    internal Comparer<T> LocalComparer { get; } = Comparer<T>.Default;

    #region IEnumerable members

    public IEnumerator<T> GetEnumerator()
    {
        SkipListNode? current = Head.Forward[0].Next;
        while (current != null)
        {
            yield return current.Value;
            current = current.Forward[0].Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region ICollection members

    public int Count { get; private set; }
    bool ICollection<T>.IsReadOnly => false;
    bool ICollection.IsSynchronized => false;
    object ICollection.SyncRoot => this;

    public void CopyTo(Array array, int index)
    {
        if (array is T[] tArray) CopyTo(tArray, index);
        else throw new ArgumentException("Generic type do not match!", nameof(array));
    }

    public void Add(T item)
    {
        unsafe
        {
            // rank 存储每一层位于插入节点的前一个节点的排名.
            Span<int> rank = stackalloc int[MaxLevel];
            // 在各个层查找节点的插入位置
            SkipListNode curr = Head;
            // 由高层向低层, 在每层寻找新节点的前驱节点 curr.
            // 完成插入后, 前驱节点的后一个节点就是新插入节点.
            for (int i = m_CurrLevel - 1; i >= 0; i--)
            {
                // 当位于顶层节点时, 运行到此处, x 还指向头结点, 未跨过任何节点, 跨度为 0.
                // 当不位于顶层节点时, 说明当上一层寻找完毕, x 刚由上一层切换到本层.
                // 由于是同一个节点, 所以排名是一样的, 记录下来, 后续在本层进行寻找时使用.
                rank[i] = i == m_CurrLevel - 1 ? 0 : rank[i + 1];
                // 沿着前进指针遍历跳跃表.
                // 若分值相同, 则比较对象.
                SkipListNode? next = curr.Forward[i].Next;
                while (next != null && LocalComparer.Compare(next.Value, item) <= 0)
                {
                    // 记录沿途跨越了多少个节点
                    rank[i] += curr.Forward[i].Span;
                    // 移动至下一个指针
                    curr = next;
                    next = curr.Forward[i].Next;
                }

                // 将 i 层里找到的前驱节点记录, 等待后续插入使用
                s_UpdateArray[i] = curr;
                // i 层查找结束, 继续从 curr 节点的下边一层开始往后寻找
            }

            // 计算随机值, 后续作为新节点的层数
            int level = GetRandomLevel();
            // 新节点的层数超出了 skipList 原有节点中最大已有层数
            // 头结点超出部分的层数将会被使用, 将其记录到 m_UpdateArray 供后续使用
            if (level > m_CurrLevel)
            {
                for (int i = m_CurrLevel; i < level; i++)
                {
                    rank[i] = 0;
                    s_UpdateArray[i] = Head;
                    s_UpdateArray[i].Forward[i].Span = Count;
                }

                // 更新表中节点最大层数
                m_CurrLevel = level;
            }

            // 创建新节点
            curr = new SkipListNode(level, item);
            // 将前面记录的指针指向新节点, 并做相应的设置
            for (var i = 0; i < level; i++)
            {
                // 设置新节点的 forward 指针
                curr.Forward[i].Next = s_UpdateArray[i].Forward[i].Next;
                // 将沿途记录的各个节点的 forward 指针指向新节点
                s_UpdateArray[i].Forward[i].Next = curr;

                // 计算新节点跨越的节点数量
                curr.Forward[i].Span = s_UpdateArray[i].Forward[i].Span - (rank[0] - rank[i]);
                //由于新节点插入，前驱节点需增加 1
                s_UpdateArray[i].Forward[i].Span = rank[0] - rank[i] + 1;
            }

            // 高层节点跨越了新节点，跨度加 1
            for (int i = level; i < m_CurrLevel; i++) s_UpdateArray[i].Forward[i].Span++;
            // 跳跃表的节点计数增一
            Count++;
        }
    }

    public bool Contains(T item) => FirstEqual_Internal(item) != null;

    /// <summary>
    /// 移除首个与 item 比较值为 0 的元素.
    /// 关于移除完全等同元素的方法, 另请参阅: <see cref="RemoveItem"/>.
    /// </summary>
    public bool Remove(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) < 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }

            s_UpdateArray[i] = curr;
        }

        SkipListNode? node = curr.Forward[0].Next;
        if (node == null || LocalComparer.Compare(node.Value, item) != 0) return false;
        RemoveNode_Internal(node);
        return true;
    }
    
    /// <summary>
    /// 移除首个完全等同于 item 的元素.
    /// 关于移除比较值为 0 元素的方法, 另请参阅: <see cref="Remove"/>.
    /// </summary>
    public bool RemoveItem(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) < 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }

            s_UpdateArray[i] = curr;
        }

        SkipListNode? node = curr.Forward[0].Next;
        while (node != null)
        {
            if (!Equals(node.Value, item))
            {
                node = node.Forward[0].Next;
            }
            else
            {
                RemoveNode_Internal(node);
                return true;
            }
        }
        return false;
    }

    private void RemoveNode_Internal(SkipListNode temp)
    {
        // 将节点 curr 从跳表中移出
        for (var i = 0; i < m_CurrLevel; i++)
            if (s_UpdateArray[i].Forward[i].Next == temp)
            {
                s_UpdateArray[i].Forward[i].Span += temp.Forward[i].Span - 1;
                s_UpdateArray[i].Forward[i].Next = temp.Forward[i].Next;
            }
            else
            {
                s_UpdateArray[i].Forward[i].Span -= 1;
            }

        while (m_CurrLevel > 1 && Head.Forward[m_CurrLevel - 1].Next == null) m_CurrLevel--;
        Count--;
    }

    public void Clear()
    {
        Array.Clear(Head.Forward, 0, Head.Forward.Length);
        Count = m_CurrLevel = 0;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        SkipListNode? current = Head.Forward[0].Item1;
        while (current != null)
        {
            array[arrayIndex++] = current.Value;
            current = current.Forward[0].Item1;
        }
    }

    #endregion

    #region IList members

    public T this[int index]
    {
        set => throw new NotSupportedException();
        get
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count, nameof(index));
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

            SkipListNode curr = Head;
            int currentIndex = -1;

            for (int i = m_CurrLevel - 1; i >= 0; i--)
            {
                SkipListNode? next = curr.Forward[i].Next;
                while (next != null && currentIndex + curr.Forward[i].Span <= index)
                {
                    currentIndex += curr.Forward[i].Span;
                    curr = next;
                    next = curr.Forward[i].Next;
                }

                if (currentIndex == index) return curr.Value;
            }

            return default!;
        }
    }

    public int IndexOf(T item)
    {
        SkipListNode curr = Head;
        var currIndex = -1;
        
        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            
            while (next != null && LocalComparer.Compare(next.Value, item) < 0)
            {
                currIndex += curr.Forward[i].Span;
                curr = next;
                next = curr.Forward[i].Next;
            }

            if (next == null || LocalComparer.Compare(next.Value, item) != 0) continue;
            
            return currIndex + curr.Forward[i].Span;
        }

        return -1;
    }

    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

    public void RemoveAt(int index)
    {
        // 如果索引无效，则返回
        if (index < 0 || index >= Count) return;
        
        SkipListNode curr = Head;
        SkipListNode? nodeToRemove = null;
        int currIndex = -1;
        
        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && currIndex + curr.Forward[i].Span < index)
            {
                currIndex += curr.Forward[i].Span;
                curr = next;
                next = curr.Forward[i].Next;
            }
            
            s_UpdateArray[i] = curr;

            if (currIndex == index) continue;
            nodeToRemove = curr.Forward[0].Next;
        }
        
        if (nodeToRemove != null) RemoveNode_Internal(nodeToRemove);
    }
    
    #endregion

    #region PrivateMethods

    internal SkipListNode? FirstEqual_Internal(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) < 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }
        }
        
        SkipListNode? node = curr.Forward[0].Next;
        if (node == null) return null;
        return LocalComparer.Compare(node.Value, item) == 0 ? node : null;
    }
    
    internal SkipListNode? LastEqual_Internal(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) <= 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }
        }
        return LocalComparer.Compare(curr.Value, item) == 0 ? curr : null;
    }
    
    internal SkipListNode? FirstGreaterThan_Internal(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) <= 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }
        }

        return curr.Forward[0].Next;
    }
    
    internal SkipListNode? FirstGreaterThanOrEqual_Internal(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) < 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }
        }

        return curr.Forward[0].Next;
    }
    
    internal SkipListNode LastLessThan_Internal(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) <= 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }
        }

        return curr;
    }
    
    internal SkipListNode LastLessThanOrEqual_Internal(T item)
    {
        SkipListNode curr = Head;

        for (int i = m_CurrLevel - 1; i >= 0; i--)
        {
            SkipListNode? next = curr.Forward[i].Next;
            while (next != null && LocalComparer.Compare(next.Value, item) < 0)
            {
                curr = next;
                next = curr.Forward[i].Next;
            }
        }

        return curr;
    }
    
    #endregion
    
    #region SkipList members
    
    public T? FirstEqual(T item)
    {
        SkipListNode? node = FirstEqual_Internal(item);
        return node == null ? default : node.Value;
    }
    
    public T? LastEqual(T item)
    {
        SkipListNode? node = LastEqual_Internal(item);
        return node == null ? default : node.Value;
    }
    
    public T? FirstGreaterThan(T item)
    {
        SkipListNode? node = FirstGreaterThan_Internal(item);
        return node == null ? default : node.Value;
    }
    
    public T? FirstGreaterThanOrEqual(T item)
    {
        SkipListNode? node = FirstGreaterThanOrEqual_Internal(item);
        return node == null ? default : node.Value;
    }
    
    public T LastLessThan(T item) => LastLessThan_Internal(item).Value;

    public T LastLessThanOrEqual(T item) => LastLessThanOrEqual_Internal(item).Value;
    
    /// <summary>
    /// 返回范围内的所有元素.
    /// 参数 lower 与参数 upper 都是闭区间.
    /// </summary>
    public IEnumerable<T> Range(T lower, T upper)
    {
        SkipListNode? current = FirstGreaterThanOrEqual_Internal(lower);
        if (current == null) yield break;
        while (LocalComparer.Compare(current.Value, upper) <= 0)
        {
            yield return current.Value;
            current = current.Forward[0].Next;
            if (current == null) yield break;
        }
    }
    
    #endregion
    
    #region Helper

    private static int GetRandomLevel()
    {
        var level = 1;
        while (level < MaxLevel &&
               (m_Random.Next(0, short.MaxValue) & ushort.MaxValue) < Probability * ushort.MaxValue
              ) ++level;
        return level;
    }

    [DebuggerDisplay("Node [{Value}]")]
    public class SkipListNode(int level, T value)
    {
        public readonly (SkipListNode? Next, int Span)[] Forward = new (SkipListNode?, int)[level];
        public T Value { get; internal set; } = value;
    }

    #endregion

    #region Fields

    private const int MaxLevel = 32;
    private const float Probability = 0.25f;
    private int m_CurrLevel;
    internal SkipListNode Head { get; } = new SkipListNode(MaxLevel, default!);

    #endregion

    #region Constructors

    public SkipList()
    {
    }
    
    public SkipList(Comparer<T> comparer) => LocalComparer = comparer;

    public SkipList(IEnumerable<T> collection)
    {
        T[] array = collection.ToArray();
        Array.Sort(array, LocalComparer);
        foreach (T item in array) Add(item);
    }
    
    public SkipList(IEnumerable<T> collection, Comparer<T> comparer)
    {
        T[] array = collection.ToArray();
        Array.Sort(array, LocalComparer = comparer);
        foreach (T item in array) Add(item);
    }

    #endregion
    
}