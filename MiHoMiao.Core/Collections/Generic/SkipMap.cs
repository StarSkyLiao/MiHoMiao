//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// 利用可重复元素跳表的字典实现.
/// [注意]: 该字典允许重复键!
/// 其增删改查等操作的平均时间复杂度都是 O(log(n)).
/// 该字典的用处是快速查找指定范围内的所有元素, 而非简单的键值对关系.
/// 例如: 查找一定范围分数内, 学生的成绩.
/// </summary>
public class SkipMap<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<(TKey Key, TValue Value)>, ICollection
    where TKey : IComparable<TKey>
{
    private readonly SkipList<(TKey Key, TValue Value)> m_SkipList;
    
    #region IEnumerable members

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        var current = m_SkipList.Head.Forward[0].Next;
        while (current != null)
        {
            yield return new KeyValuePair<TKey, TValue>(current.Value.Key, current.Value.Value);
            current = current.Forward[0].Next;
        }
    }

    public IEnumerator<(TKey Key, TValue Value)> GetEnumerator() => m_SkipList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region ICollection members

    public bool Remove(KeyValuePair<TKey, TValue> item) => m_SkipList.RemoveItem((item.Key, item.Value));

    public int Count => m_SkipList.Count;
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
    bool ICollection<(TKey Key, TValue Value)>.IsReadOnly => false;
    bool ICollection.IsSynchronized => false;
    object ICollection.SyncRoot => this;

    public void CopyTo(Array array, int index)
    {
        if (array is (TKey Key, TValue Value)[] tArray) CopyTo(tArray, index);
        else throw new ArgumentException("Generic type do not match!", nameof(array));
    }

    public void Add(TKey key, TValue value) => m_SkipList.Add((key, value));
    public void Add((TKey Key, TValue Value) item) => m_SkipList.Add(item);
    public void Add(KeyValuePair<TKey, TValue> item) => m_SkipList.Add((item.Key, item.Value));

    public bool Contains((TKey Key, TValue Value) item) => m_SkipList.Contains(item);
    public bool Contains(KeyValuePair<TKey, TValue> item) => m_SkipList.Contains((item.Key, item.Value));
    
    public bool Remove((TKey Key, TValue Value) item) => m_SkipList.RemoveItem(item);

    public void Clear() => m_SkipList.Clear();

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => throw new NotSupportedException();
    public void CopyTo((TKey Key, TValue Value)[] array, int arrayIndex) => m_SkipList.CopyTo(array, arrayIndex);

    #endregion
    
    #region SkipList members

    public (TKey Key, TValue Value)? FirstEqual(TKey key)
        => m_SkipList.FirstEqual((key, default!));
    
    public (TKey Key, TValue Value)? LastEqual(TKey key)
        => m_SkipList.LastEqual((key, default!));
    
    public (TKey Key, TValue Value)? FirstGreaterThan(TKey key)
        => m_SkipList.FirstGreaterThan((key, default!));
    
    public (TKey Key, TValue Value)? FirstGreaterThanOrEqual(TKey key)
        => m_SkipList.FirstGreaterThanOrEqual((key, default!));
    
    public (TKey Key, TValue Value) LastLessThan(TKey key)
        => m_SkipList.LastLessThan((key, default!));

    public (TKey Key, TValue Value) LastLessThanOrEqual(TKey key)
        => m_SkipList.LastLessThanOrEqual((key, default!));

    /// <summary>
    /// 返回一定键范围内的所有键值对.
    /// 参数 lower 与参数 upper 都是闭区间.
    /// </summary>
    public IEnumerable<(TKey Key, TValue Value)> Range(TKey lower, TKey upper)
        => m_SkipList.Range((lower, default!), (upper, default!));

    /// <summary>
    /// 返回一定键范围内的所有键.
    /// 参数 lower 与参数 upper 都是闭区间.
    /// </summary>
    public IEnumerable<TKey> RangeKey(TKey lower, TKey upper)
    {
        var current = m_SkipList.FirstGreaterThanOrEqual_Internal((lower, default!));
        if (current == null) yield break;
        while (m_SkipList.LocalComparer.Compare(current.Value, (upper, default!)) <= 0)
        {
            yield return current.Value.Key;
            current = current.Forward[0].Next;
            if (current == null) yield break;
        }
    }
    
    /// <summary>
    /// 返回一定键范围内的所有值.
    /// 参数 lower 与参数 upper 都是闭区间.
    /// </summary>
    public IEnumerable<TValue> RangeValue(TKey lower, TKey upper)
    {
        var current = m_SkipList.FirstGreaterThanOrEqual_Internal((lower, default!));
        if (current == null) yield break;
        while (m_SkipList.LocalComparer.Compare(current.Value, (upper, default!)) <= 0)
        {
            yield return current.Value.Value;
            current = current.Forward[0].Next;
            if (current == null) yield break;
        }
    }
    
    #endregion
    
    #region Constructors

    public SkipMap() => m_SkipList = new SkipList<(TKey Key, TValue Value)>(
        Comparer<(TKey Key, TValue Value)>.Create(
            (self, other) => Comparer.Default.Compare(self.Key, other.Key)
        )
    );

    public SkipMap(Comparer<TKey> comparer)
        => m_SkipList = new SkipList<(TKey Key, TValue Value)>(
            Comparer<(TKey Key, TValue Value)>.Create(
                (self, other) => comparer.Compare(self.Key, other.Key)
            )
        );

    public SkipMap(IEnumerable<(TKey, TValue)> collection) =>
        m_SkipList = new SkipList<(TKey Key, TValue Value)>(collection);

    public SkipMap(IEnumerable<(TKey, TValue)> collection, Comparer<TKey> comparer)
        => m_SkipList = new SkipList<(TKey Key, TValue Value)>(
            collection, 
            Comparer<(TKey Key, TValue Value)>.Create(
                (self, other) => comparer.Compare(self.Key, other.Key)
            )
        );

    #endregion

    #region Dictionary members

    public bool ContainsKey(TKey key) => m_SkipList.FirstEqual_Internal((key, default!)) != null;

    public bool Remove(TKey key) => m_SkipList.Remove((key, default!));

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        var node = m_SkipList.FirstEqual_Internal((key, default!));
        if (node == null)
        {
            value = default;
            return false;
        }
        else
        {
            value = node.Value.Value;
            return true;
        }
    }

    public TValue this[TKey key]
    {
        get => m_SkipList.FirstEqual_Internal((key, default!))!.Value.Value;
        set => m_SkipList.FirstEqual_Internal((key, default!))!.Value = (key, value);
    }

    [field: AllowNull, MaybeNull]
    public ICollection<TKey> Keys => field ??= new KeyCollection(this);
    
    [field: AllowNull, MaybeNull]
    public ICollection<TValue> Values => field ??= new ValueCollection(this);
    
    #region KeyValueSet

    public sealed class KeyCollection : ICollection<TKey>, ICollection
    {
        private readonly SkipMap<TKey, TValue> m_Dict;
        
        internal KeyCollection(SkipMap<TKey, TValue> dictionary) => m_Dict = dictionary;

        public void Add(TKey item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(TKey item) => throw new NotSupportedException();

        public void CopyTo(TKey[] array, int arrayIndex) => throw new NotSupportedException();

        public bool Remove(TKey item) => throw new NotSupportedException();

        public int Count => m_Dict.Count;
        bool ICollection<TKey>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => ((ICollection) m_Dict).SyncRoot;
        
        void ICollection.CopyTo(Array array, int arrayIndex) => throw new NotSupportedException();
        
        public IEnumerator<TKey> GetEnumerator()
        {
            var current = m_Dict.m_SkipList.Head.Forward[0].Next;
            while (current != null)
            {
                yield return current.Value.Key;
                current = current.Forward[0].Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }
    
    public sealed class ValueCollection : ICollection<TValue>, ICollection
    {
        private readonly SkipMap<TKey, TValue> m_Dict;
        
        internal ValueCollection(SkipMap<TKey, TValue> dictionary) => m_Dict = dictionary;

        public void Add(TValue item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(TValue item) => throw new NotSupportedException();

        public void CopyTo(TValue[] array, int arrayIndex) => throw new NotSupportedException();

        public bool Remove(TValue item) => throw new NotSupportedException();

        public int Count => m_Dict.Count;
        bool ICollection<TValue>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => ((ICollection) m_Dict).SyncRoot;
        
        void ICollection.CopyTo(Array array, int arrayIndex) => throw new NotSupportedException();
        
        public IEnumerator<TValue> GetEnumerator()
        {
            var current = m_Dict.m_SkipList.Head.Forward[0].Next;
            while (current != null)
            {
                yield return current.Value.Value;
                current = current.Forward[0].Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }
    

    #endregion
    
    #endregion
    
}