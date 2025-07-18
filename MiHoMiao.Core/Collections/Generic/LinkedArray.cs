//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 2.0
//------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// 链表的数组实现.
/// 使用一个数组来表示链表, 每一个元素都包含了前一个合后一个节点的索引值.
/// 数组的第 0 个元素为一个虚拟节点, 该节点不可存储值, 其下一个元素为链表的开始.
/// 注意: 指定元素的索引值并不固定, 可能会发生变化以节省内存空间.
/// </summary>
public sealed class LinkedArray<T> : IList<T>, IList, IReadOnlyList<T>
{
    internal record struct Node(T Value, int Front, int Next);
    
    internal Node[] Items;
    internal int Version;

    private static readonly Node[] s_EmptyArray = [new Node(default!, 0, 0)];

    #region Constructor

    /// <summary>
    /// 创建一个无序动态数组的实例
    /// </summary>
    public LinkedArray() => Items = s_EmptyArray;
    
    /// <summary>
    /// 创建一个初始容量为 capacity 的无序动态数组的实例
    /// </summary>
    public LinkedArray(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        Items = capacity == 0 ? s_EmptyArray : new Node[capacity];
    }
    
    /// <summary>
    /// 从可枚举 collection 中实例化一个无序动态数组
    /// </summary>
    public LinkedArray(IEnumerable<T>? collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        
        if (collection is ICollection<T> objs)
        {
            int count = objs.Count;
            if (count == 0) Items = s_EmptyArray;
            else
            {
                Items = new Node[count];
                AddRange(collection);
            }
        }
        else
        {
            Items = s_EmptyArray;
            AddRange(collection);
        }
    }

    #endregion
    
    /// <summary>
    /// 无序动态数组的容量大小,
    /// 注意: 因为存在一个虚拟头节点, 该值 = 实际可以存储的元素数量 + 1.
    /// </summary>
    public int Capacity
    {
        get => Items.Length;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, Count);
            if (value == Items.Length) return;
            if (value > 0)
            {
                Node[] destinationArray = new Node[value];
                if (Count > 0) Array.Copy(Items, destinationArray, Count + 1);
                Items = destinationArray;
            }
            else Items = s_EmptyArray;
        }
    }
    
    #region PrivateListMethods

    private void Grow(int capacity) => Capacity = GetNewCapacity(++capacity);

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetNewCapacity(int capacity)
    {
        int newCapacity = Items.Length == 0 ? 4 : 2 * Items.Length - 1;
        if ((uint) newCapacity > 2147483591U) newCapacity = 2147483591;
        if (newCapacity < capacity) newCapacity = capacity;
        return newCapacity;
    }
    
    private void RemoveAt_Internal(int index)
    {
        ArgumentOutOfRangeException.ThrowIfZero(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)index, (uint)Count);

        int front = Items[index].Front;
        int next = Items[index].Next;
        Items[front].Next = next;
        Items[next].Front = front;

        if (index != Count)
        {
            front = Items[Count].Front;
            next = Items[Count].Next;
            Items[index] = Items[Count];
            Items[front].Next = index;
            Items[next].Front = index;
        }

        --Count;
        
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) Items[Count] = default!;
        ++Version;
    }

    private static void IfNullAndNullsAreIllegalThenThrow(object? value)
    {
        if (default(T) == null || value != null) return;
        throw new ArgumentNullException(nameof(value));
    }
    
    private static bool IsCompatibleObject(object? value)
    {
        if (value is T) return true;
        return value == null && default (T) == null;
    }
    
    #endregion
    
    #region IEnumerableMethods
    
    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public struct Enumerator : IEnumerator<T>
    {
        private readonly LinkedArray<T> m_LinkedArray;
        private readonly int m_Version;
        private T? m_Current;
        private int m_Index;
        
        internal Enumerator(LinkedArray<T> linkedArray)
        {
            m_LinkedArray = linkedArray;
            m_Version = linkedArray.Version;
            m_Current = default;
            m_Index = 0;
        }

        public T Current => m_Current!;

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (m_Version != m_LinkedArray.Version) throw new InvalidOperationException();
            m_Index = m_LinkedArray.Items[m_Index].Next;
            if (m_Index == 0) return false;
            m_Current = m_LinkedArray.Items[m_Index].Value;
            return true;
        }

        void IEnumerator.Reset()
        {
            if (m_Version != m_LinkedArray.Version) throw new InvalidOperationException();

            m_Current = default;
            m_Index = 0;
        }

        public void Dispose()
        {
        }
    }
    
    #endregion
    
    #region ICollectionMembers

    public int Count { get; private set; }

    bool ICollection<T>.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        ++Version;
        int front = Count;
        int next = Items[front].Next;
        int current = front + 1;
        if (current >= Items.Length) Grow(current);
        Count = current;
        Items[current] = new Node(item, front, next);
        Items[front].Next = current;
        Items[next].Front = current;
    }

    public void Clear()
    {
        ++Version;
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            int size = Count;
            Count = 0;
            if (size <= 0) return;
            Array.Clear(Items, 0, Items.Length);
        }
        else
        {
            Items[0] = new Node(default!, 0, 0);
            Count = 0;
        }
    }
    
    public bool Contains(T item) => Count != 0 && IndexOf(item) >= 0;
    
    public void CopyTo(T[] array) => CopyTo(array, 0);
    
    public void CopyTo(T[] array, int arrayIndex)
    {
        int node = Items[0].Next;
        while (node != 0)
        {
            array[arrayIndex++] = Items[node].Value!;
            node = Items[node].Next;
        }
    }

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index < 0) return false;
        RemoveAt_Internal(index);
        return true;
    }
    
    public bool Remove(LinkedArrayNode<T> node)
    {
        if (node.Target != this) return false;
        if (node.Version != Version) return false;
        RemoveAt_Internal(node.Index);
        return true;
    }
    
    void ICollection.CopyTo(Array array, int index)
    {
        if (array != null && array.Rank != 1) throw new ArgumentException("RankMultiDimNotSupported");
        Array.Copy(Items, 0, array!, index, Count);
    }

    #endregion
    
    #region IList<T>Members
    
    [Obsolete("LinkedArray shouldn't use this method! It will read raw data which may not satisfy you needs.")]
    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
            return Items[index + 1].Value;
        }
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
            Items[index + 1].Value = value;
            ++Version;
        }
    }
    
    public int IndexOf(T item)
    {
        for (int index = 1; index < Count; ++index)
            if (Items[index].Value!.Equals(item)) return index;
        return -1;
    }

    [Obsolete("LinkedArray shouldn't use this method! It will be converted to Add(T item) instead.")]
    public void Insert(int index, T item)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
        Add(item);
    }
    
    void IList.RemoveAt(int index) => RemoveAt_Internal(++index);
    void IList<T>.RemoveAt(int index) => RemoveAt_Internal(++index);

    #endregion
    
    #region IListMembers
    
    bool IList.IsReadOnly => false;
    
    [Obsolete("LinkedArray shouldn't use this method! It will read raw data which may not satisfy you needs.")]
    object? IList.this[int index]
    {
        get => this[index];
        set
        {
            IfNullAndNullsAreIllegalThenThrow(value);
            this[index] = (T)value!;
        }
    }

    [Obsolete("Don't use this method unless you know what it does.")]
    int IList.Add(object? value)
    {
        IfNullAndNullsAreIllegalThenThrow(value);
        Add((T)value!);
        return Count;
    }

    bool IList.Contains(object? value) => IsCompatibleObject(value) && Contains((T)value!);

    bool IList.IsFixedSize => true;

    int IList.IndexOf(object? value) => IsCompatibleObject(value) ? IndexOf((T)value!) : -1;

    [Obsolete("Don't use this method unless you know what it does.")]
    void IList.Insert(int index, object? value)
    {
        IfNullAndNullsAreIllegalThenThrow(value);
        Insert(index, (T)value!);
    }

    void IList.Remove(object? value)
    {
        if (!IsCompatibleObject(value)) return;
        Remove((T)value!);
    }
    
    #endregion
    
    #region ListMethods
    
    /// <summary>
    /// 将指定集合的元素添加到无序动态数组的末尾.
    /// </summary>
    public void AddRange(IEnumerable<T>? collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        if (collection is ICollection<T> objs) EnsureCapacity(Count + objs.Count);
        foreach (T obj in collection) Add(obj);
    }
    
    /// <summary>
    /// 返回当前链式数组的只读包装器.
    /// </summary>
    public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>(this);
    
    /// <summary>
    /// 将当前链式数组中的元素转换为另一种类型,
    /// 并返回包含已转换元素的列表.
    /// </summary>
    public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput>? converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        List<TOutput> outputList = new List<TOutput>(Count);
        for (int index = 1; index <= Count; ++index)
            outputList.Add(converter(Items[index].Value));
        return outputList;
    }
    
    /// <summary>
    /// 确保此列表的容量至少为指定的容量.
    /// 如果当前容量小于 capacity, 则至少增加到指定的容量
    /// </summary>
    public int EnsureCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (Items.Length < capacity) Grow(capacity);
        return Items.Length;
    }
    
    /// <summary>
    /// 返回是否存在满足条件的元素.
    /// </summary>
    public bool Exists(Predicate<T> match) => FindIndex(match) != -1;
    
    /// <summary>
    /// 返回首个满足条件的元素.
    /// </summary>
    public T? Find(Predicate<T>? match)
    {
        ArgumentNullException.ThrowIfNull(match);
        for (int index = 1; index <= Count; ++index)
            if (match(Items[index].Value)) return Items[index].Value;
        return default;
    }
    
    /// <summary>
    /// 返回所有满足条件的元素.
    /// </summary>
    public UnorderedList<T> FindAll(Predicate<T>? match)
    {
        ArgumentNullException.ThrowIfNull(match);
        UnorderedList<T> all = [];
        for (int index = 1; index <= Count; ++index)
            if (match(Items[index].Value)) all.Add(Items[index].Value);
        return all;
    }
    
    /// <summary>
    /// 返回首个满足条件元素的索引值.
    /// 不存在时返回 -1.
    /// </summary>
    public int FindIndex(Predicate<T>? match) => FindIndex(0, Count, match);
    
    /// <summary>
    /// 从 startIndex 开始, 返回首个满足条件元素的索引值.
    /// 不存在时返回 -1.
    /// </summary>
    public int FindIndex(int startIndex, Predicate<T>? match) 
        => FindIndex(startIndex, Count - startIndex, match);
    
    /// <summary>
    /// 从 startIndex 开始, 向后查找 count 个元素, 返回首个满足条件元素的索引值.
    /// 不存在时返回 -1.
    /// </summary>
    public int FindIndex(int startIndex, int count, Predicate<T>? match)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)startIndex, (uint)Count);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, Count - startIndex);
        ArgumentNullException.ThrowIfNull(match);
        int num = (++startIndex) + count;
        for (int index = startIndex; index < num; ++index)
            if (match(Items[index].Value)) return index;
        return -1;
    }

    /// <summary>
    /// 对所有元素执行一次 action 操作.
    /// </summary>
    public void ForEach(Action<T>? action)
    {
        ArgumentNullException.ThrowIfNull(action);
        int version = Version;
        for (int index = 1; index <= Count && version == Version; ++index)
            action(Items[index].Value);
        if (version == Version) return;
        throw new InvalidOperationException("EnumFailedVersion");
    }
    
    // /// <summary>
    // /// 移除所有满足条件的元素.
    // /// </summary>
    // public int RemoveAll(Predicate<T>? match)
    // {
    //     ArgumentNullException.ThrowIfNull(match);
    //     int index1 = 0;
    //     while (index1 < Count && !match(m_Items[index1].Value)) ++index1;
    //     if (index1 >= Count) return 0;
    //     int index2 = index1 + 1;
    //     while (index2 < Count)
    //     {
    //         while (index2 < Count && match(m_Items[index2].Value)) ++index2;
    //         if (index2 < Count) m_Items[index1++] = m_Items[index2++];
    //     }
    //     if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) 
    //         Array.Clear(m_Items, index1, Count - index1);
    //     int num = Count - index1;
    //     Count = index1;
    //     ++m_Version;
    //     return num;
    // }
    
    /// <summary>
    /// 如果实际占用不超过容量的 90%, 则将容量设置为实际的占用大小.
    /// </summary>
    public void TrimExcess()
    {
        if (Count >= (int) (Items.Length * 0.9)) return;
        Capacity = Count + 1;
    }
    
    /// <summary>
    /// 返回是否所有元素全部满足目标表达式.
    /// </summary>
    public bool TrueForAll(Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(match);
        for (int index = 1; index <= Count; ++index)
            if (!match(Items[index].Value)) return false;
        return true;
    }
    
    #endregion
    
    #region Queue<T>

    public void AddLast(T item) => Add(item);

    public void Enqueue(T item) => Add(item);
    
    public void AddFirst(T item)
    {
        if (Count == 0) Add(item);
        else AddBefore(new LinkedArrayNode<T>(Items[0].Next, this), item);
    }

    public bool TryDequeue([MaybeNullWhen(false)]out T item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        int index = Items[0].Next;
        item = Items[index].Value;
        RemoveAt_Internal(index);
        return true;
    }

    public T? Dequeue()
    {
        if (Count == 0) return default;
        int index = Items[0].Next;
        T result = Items[index].Value;
        RemoveAt_Internal(index);
        return result;
    }

    public bool RemoveFirst()
    {
        if (Count == 0) return false;
        RemoveAt_Internal(Items[0].Next);
        return true;
    }
    
    public bool RemoveLast()
    {
        if (Count == 0) return false;
        RemoveAt_Internal(Items[0].Front);
        return true;
    }

    #endregion
    
    #region LinkedList<T>

    public LinkedArrayNode<T>? First
    {
        get
        {
            if (Count == 0) return null;
            return new LinkedArrayNode<T>(Items[0].Next, this);
        }
    }
    
    public T? FirstValue => Count == 0 ? default : Items[Items[0].Next].Value;

    public LinkedArrayNode<T>? Last
    {
        get
        {
            if (Count == 0) return null;
            return new LinkedArrayNode<T>(Items[0].Front, this);
        }
    }
    
    public T? LastValue => Count == 0 ? default : Items[Items[0].Front].Value;

    public LinkedArrayNode<T>? Find(T item)
    {
        if (Count == 0) return null;
        int node = Items[0].Next;
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        while (node != 0)
        {
            if (comparer.Equals(Items[node].Value, item)) 
                return new LinkedArrayNode<T>(node, this);
            node = Items[node].Next;
        }
        return null;
    }
    
    public LinkedArrayNode<T>? FindLast(T item)
    {
        if (Count == 0) return null;
        int node = Items[0].Front;
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        while (node != 0)
        {
            if (comparer.Equals(Items[node].Value, item)) 
                return new LinkedArrayNode<T>(node, this);
            node = Items[node].Front;
        }
        return null;
    }

    public LinkedArrayNode<T> AddAfter(LinkedArrayNode<T> node, T value)
    {
        if (node.Target != this || node.Version != Version)
            throw new InvalidOperationException("VersionNotMatch");
        ++Version;
        int front = node.Index;
        int next = Items[front].Next;
        int current = Count + 1;
        if (current >= Items.Length) Grow(current);
        Count = current;
        Items[current] = new Node(value, front, next);
        Items[front].Next = current;
        Items[next].Front = current;
        return new LinkedArrayNode<T>(current, this);
    }
    
    public LinkedArrayNode<T> AddBefore(LinkedArrayNode<T> node, T value)
    {
        if (node.Target != this || node.Version != Version)
            throw new InvalidOperationException("VersionNotMatch");
        ++Version;
        int next = node.Index;
        int front = Items[next].Front;
        int current = Count + 1;
        if (current >= Items.Length) Grow(current);
        Count = current;
        Items[current] = new Node(value, front, next);
        Items[front].Next = current;
        Items[next].Front = current;
        return new LinkedArrayNode<T>(current, this);
    }
    
    #endregion
    
}