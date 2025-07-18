//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// 无序动态数组的实现与<see cref="T:System.Collections.Generic.List`1" />几乎一致.
/// 与之不同的是, 在删除元素时会将最后一个元素移动到被删除元素的空位, 以此降低删除操作的耗时.
/// 在不关心数据的顺序时, 可以使用该容器取代<see cref="T:System.Collections.Generic.List`1" />.
/// </summary>
public sealed class UnorderedList<T> : IList<T>, IList, IReadOnlyList<T>
{
    private T[] m_Items;
    private int m_Version;

    private static readonly T[] s_EmptyArray = [];
    
    /// <summary>
    /// 创建一个无序动态数组的实例
    /// </summary>
    public UnorderedList() => m_Items = s_EmptyArray;
    
    /// <summary>
    /// 创建一个初始容量为 capacity 的无序动态数组的实例
    /// </summary>
    public UnorderedList(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        m_Items = capacity == 0 ? s_EmptyArray : new T[capacity];
    }
    
    /// <summary>
    /// 从可枚举 collection 中实例化一个无序动态数组
    /// </summary>
    public UnorderedList(IEnumerable<T>? collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection is ICollection<T> objs)
        {
            int count = objs.Count;
            if (count == 0)
            {
                m_Items = s_EmptyArray;
            }
            else
            {
                m_Items = new T[count];
                objs.CopyTo(m_Items, 0);
                Count = count;
            }
        }
        else
        {
            m_Items = s_EmptyArray;
            foreach (T obj in collection) Add(obj);
        }
    }
    
    /// <summary>
    /// 无序动态数组的容量大小
    /// </summary>
    public int Capacity
    {
        get => m_Items.Length;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, Count);
            if (value == m_Items.Length) return;
            if (value > 0)
            {
                T[] destinationArray = new T[value];
                if (Count > 0) Array.Copy(m_Items, destinationArray, Count);
                m_Items = destinationArray;
            }
            else m_Items = s_EmptyArray;
        }
    }

    #region PrivateListMethods

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddWithResize(T item)
    {
        int size = Count;
        Grow(size + 1);
        Count = size + 1;
        m_Items[size] = item;
    }
    
    private void Grow(int capacity) => Capacity = GetNewCapacity(capacity);

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetNewCapacity(int capacity)
    {
        int newCapacity = m_Items.Length == 0 ? 4 : 2 * m_Items.Length;
        if ((uint) newCapacity > 2147483591U) newCapacity = 2147483591;
        if (newCapacity < capacity) newCapacity = capacity;
        return newCapacity;
    }
    
    private void RemoveAt_Internal(int index)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
        --Count;
        if (index < Count) m_Items[index] = m_Items[Count];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) m_Items[Count] = default!;
        ++m_Version;
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
    
    #endregion
    
    #region ICollectionMembers

    public int Count { get; private set; }

    bool ICollection<T>.IsReadOnly => false;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        ++m_Version;
        T[] items = m_Items;
        int size = Count;
        if ((uint) size < (uint) items.Length)
        {
            Count = size + 1;
            items[size] = item;
        }
        else AddWithResize(item);
    }

    public void Clear()
    {
        ++m_Version;
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            int size = Count;
            Count = 0;
            if (size <= 0) return;
            Array.Clear(m_Items, 0, size);
        }
        else Count = 0;
    }
    
    public bool Contains(T item) => Count != 0 && IndexOf(item) >= 0;
    
    public void CopyTo(T[] array) => CopyTo(array, 0);
    
    public void CopyTo(T[] array, int arrayIndex) 
        => Array.Copy(m_Items, 0, array, arrayIndex, Count);

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index < 0) return false;
        RemoveAt_Internal(index);
        return true;
    }
    
    void ICollection.CopyTo(Array array, int index)
    {
        if (array != null && array.Rank != 1) throw new ArgumentException("RankMultiDimNotSupported");
        Array.Copy(m_Items, 0, array!, index, Count);
    }

    #endregion

    #region IList<T>Members
    
    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
            return m_Items[index];
        }
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
            m_Items[index] = value;
            ++m_Version;
        }
    }
    
    public int IndexOf(T item) => Array.IndexOf(m_Items, item, 0, Count);
    
    [Obsolete("UnorderedList shouldn't use this method! It will be changed to Add(T item) instead.")]
    public void Insert(int index, T item)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
        Add(item);
    }

    [Obsolete("Don't use this method unless you know what it does.")]
    public void RemoveAt(int index) => RemoveAt_Internal(index);
    
    #endregion

    #region IListMembers
    
    bool IList.IsReadOnly => false;
    
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
        Add((T) value!);
        return Count - 1;
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
        if (collection is ICollection<T> objs)
        {
            int count = objs.Count;
            if (count <= 0)
                return;
            if (m_Items.Length - Count < count)
                Grow(checked (Count + count));
            objs.CopyTo(m_Items, Count);
            Count += count;
            ++m_Version;
        }
        else
        {
            foreach (T obj in collection) Add(obj);
        }
    }
    
    /// <summary>
    /// 返回当前无序动态数组的只读包装器.
    /// </summary>
    public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>(this);
    
    /// <summary>
    /// 将当前无序动态数组中的元素转换为另一种类型,
    /// 并返回包含已转换元素的无序动态数组.
    /// </summary>
    public UnorderedList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput>? converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        UnorderedList<TOutput> outputList = new UnorderedList<TOutput>(Count);
        for (int index = 0; index < Count; ++index)
            outputList.m_Items[index] = converter(m_Items[index]);
        outputList.Count = Count;
        return outputList;
    }
    
    /// <summary>
    /// 确保此列表的容量至少为指定的容量.
    /// 如果当前容量小于 capacity, 则至少增加到指定的容量
    /// </summary>
    public int EnsureCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (m_Items.Length < capacity) Grow(capacity);
        return m_Items.Length;
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
        for (int index = 0; index < Count; ++index)
            if (match(m_Items[index])) return m_Items[index];
        return default;
    }
    
    /// <summary>
    /// 返回所有满足条件的元素.
    /// </summary>
    public UnorderedList<T> FindAll(Predicate<T>? match)
    {
        ArgumentNullException.ThrowIfNull(match);
        UnorderedList<T> all = [];
        for (int index = 0; index < Count; ++index)
            if (match(m_Items[index])) all.Add(m_Items[index]);
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
        int num = startIndex + count;
        for (int index = startIndex; index < num; ++index)
            if (match(m_Items[index])) return index;
        return -1;
    }

    /// <summary>
    /// 对所有元素执行一次 action 操作.
    /// </summary>
    public void ForEach(Action<T>? action)
    {
        ArgumentNullException.ThrowIfNull(action);
        int version = m_Version;
        for (int index = 0; index < Count && version == m_Version; ++index)
            action(m_Items[index]);
        if (version == m_Version) return;
        throw new InvalidOperationException("EnumFailedVersion");
    }
    
    /// <summary>
    /// 移除所有满足条件的元素.
    /// </summary>
    public int RemoveAll(Predicate<T>? match)
    {
        ArgumentNullException.ThrowIfNull(match);
        int index1 = 0;
        while (index1 < Count && !match(m_Items[index1])) ++index1;
        if (index1 >= Count) return 0;
        int index2 = index1 + 1;
        while (index2 < Count)
        {
            while (index2 < Count && match(m_Items[index2])) ++index2;
            if (index2 < Count) m_Items[index1++] = m_Items[index2++];
        }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) 
            Array.Clear(m_Items, index1, Count - index1);
        int num = Count - index1;
        Count = index1;
        ++m_Version;
        return num;
    }
    
    /// <summary>
    /// 转为数组返回.
    /// </summary>
    public T[] ToArray()
    {
        if (Count == 0) return s_EmptyArray;
        T[] destinationArray = new T[Count];
        Array.Copy(m_Items, destinationArray, Count);
        return destinationArray;
    }
    
    /// <summary>
    /// 转为列表返回.
    /// </summary>
    public List<T> ToList()
    {
        if (Count == 0) return [];
        List<T> destinationList = [..m_Items];
        return destinationList;
    }
    
    /// <summary>
    /// 如果实际占用不超过容量的 90%, 则将容量设置为实际的占用大小.
    /// </summary>
    public void TrimExcess()
    {
        if (Count >= (int) (m_Items.Length * 0.9)) return;
        Capacity = Count;
    }
    
    /// <summary>
    /// 返回是否所有元素全部满足目标表达式.
    /// </summary>
    public bool TrueForAll(Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(match);
        for (int index = 0; index < Count; ++index)
            if (!match(m_Items[index])) return false;
        return true;
    }
    
    #endregion
    
    public struct Enumerator : IEnumerator<T>
    {
        private readonly UnorderedList<T> m_List;
        private int m_Index;
        private readonly int m_Version;

        public T Current { get; private set; }
        
        internal Enumerator(UnorderedList<T> list)
        {
            m_List = list;
            m_Index = 0;
            m_Version = list.m_Version;
            Current = default!;
        }
      
        public void Dispose()
        {
        }

        public bool MoveNext()
        { 
            UnorderedList<T> list = m_List;
            if (m_Version != list.m_Version || (uint) m_Index >= (uint) list.Count) return MoveNextRare();
            Current = list.m_Items[m_Index];
            ++m_Index;
            return true;
        }

        private bool MoveNextRare()
        {
            if (m_Version != m_List.m_Version) throw new InvalidOperationException("EnumFailedVersion");
            m_Index = m_List.Count + 1;
            Current = default!;
            return false;
        }

        object? IEnumerator.Current
        {
            get
            {
                if (m_Index == 0 || m_Index == m_List.Count + 1)
                    throw new InvalidOperationException("EnumOpCantHappen");
                return Current;
            }
        }

        void IEnumerator.Reset()
        {
            if (m_Version != m_List.m_Version) throw new InvalidOperationException("EnumFailedVersion");
            m_Index = 0;
            Current = default!;
        }
    }

}