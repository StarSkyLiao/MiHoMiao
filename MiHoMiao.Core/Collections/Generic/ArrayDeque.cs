//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// An implementation of Deque.
/// Based on <see cref="T:System.Collections.Generic.Queue`1"/>.
/// </summary>
public class ArrayDeque<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>
{
    private T[] m_Array;
    private int m_Head;
    private int m_Tail;
    private int m_Version;

    public ArrayDeque() => m_Array = [];

    public ArrayDeque(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        m_Array = new T[capacity];
    }

    public ArrayDeque(IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        m_Array = collection.ToArray();
        Count = m_Array.Length;
        m_Tail = Count - 1;
    }

    #region ICollection<T> Members

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;
    
    /// <summary>
    /// Copies the elements to a new array.
    /// </summary>
    public T[] ToArray()
    {
        if (Count == 0) return [];
        T[] destinationArray = new T[Count];
        if (m_Head < m_Tail)
        {
            Array.Copy(m_Array, m_Head, destinationArray, 0, Count);
        }
        else
        {
            Array.Copy(m_Array, m_Head, destinationArray, 0, m_Array.Length - m_Head);
            Array.Copy(m_Array, 0, destinationArray, m_Array.Length - m_Head, m_Tail);
        }

        return destinationArray;
    }
    
    /// <summary>
    /// Determines whether an element is in the collection.
    /// </summary>
    public bool Contains(T item)
    {
        if (Count == 0) return false;
        if (m_Head < m_Tail) return Array.IndexOf(m_Array, item, m_Head, Count) >= 0;
        return Array.IndexOf(m_Array, item, m_Head, m_Array.Length - m_Head) >= 0 ||
               Array.IndexOf(m_Array, item, 0, m_Tail) >= 0;
    }

    public bool Remove(T item) => throw new InvalidOperationException("Deque does not support remove.");

    public int Count { get; private set; }
    public bool IsReadOnly => false;

    #endregion
    
    #region IReadOnlyCollection<T> Members
    
    public void Clear()
    {
        if (Count != 0)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (m_Head < m_Tail)
                {
                    Array.Clear(m_Array, m_Head, Count);
                }
                else
                {
                    Array.Clear(m_Array, m_Head, m_Array.Length - m_Head);
                    Array.Clear(m_Array, 0, m_Tail);
                }
            }

            Count = 0;
        }

        m_Head = 0;
        m_Tail = 0;
        ++m_Version;
    }
    
    void ICollection.CopyTo(Array array, int index) => CopyTo((T[])array, index);
    
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array, nameof(array));
        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < Count)
            throw new ArgumentOutOfRangeException(nameof(array));
        int size = Count;
        if (size == 0)
            return;
        int length1 = Math.Min(m_Array.Length - m_Head, size);
        Array.Copy(m_Array, m_Head, array, arrayIndex, length1);
        int length2 = size - length1;
        if (length2 <= 0)
            return;
        Array.Copy(m_Array, 0, array, arrayIndex + m_Array.Length - m_Head, length2);
    }
    
    #endregion
    
    #region IEnumerable<T> Members
    
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();
    
        /// <summary>
    /// Enumerates the elements of a <see cref="T:MiHoMiao.Core.Collections.Generic.ArrayDeque`1" />.
    /// </summary>
    public struct Enumerator : IEnumerator<T>
    {
        private readonly ArrayDeque<T> m_ArrayDeque;
        private readonly int m_Version;
        private int m_Index;
        private T m_CurrentElement;

        internal Enumerator(ArrayDeque<T> arrayDeque)
        {
            m_ArrayDeque = arrayDeque;
            m_Version = arrayDeque.m_Version;
            m_Index = -1;
            m_CurrentElement = default!;
        }

        /// <summary>
        /// Releases all resources used by the Enumerator.
        /// </summary>
        public void Dispose()
        {
            m_Index = -2;
            m_CurrentElement = default!;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the ArrayDeque.
        /// </summary>
        public bool MoveNext()
        {
            if (m_Version != m_ArrayDeque.m_Version) throw new InvalidOperationException("Enum Failed Version!");
            if (m_Index == -2) return false;
            ++m_Index;
            if (m_Index == m_ArrayDeque.Count)
            {
                m_Index = -2;
                m_CurrentElement = default!;
                return false;
            }

            T[] array = m_ArrayDeque.m_Array;
            var length = (uint)array.Length;
            var index = (uint)(m_ArrayDeque.m_Head + m_Index);
            if (index >= length)
                index -= length;
            m_CurrentElement = array[(int)index];
            return true;
        }

        /// <summary>
        /// Gets the element at the current position of the enumerator.
        /// </summary>
        public T Current
        {
            get
            {
                if (m_Index < 0) throw new InvalidOperationException("Enumeration Not Started Or Ended!");
                return m_CurrentElement;
            }
        }
        

        /// <summary>
        /// Gets the element at the current position of the enumerator.
        /// </summary>
        object? IEnumerator.Current => Current;

        /// <summary>
        /// Sets the enumerator to its initial position,
        /// which is before the first element in the collection.
        /// </summary>
        void IEnumerator.Reset()
        {
            if (m_Version != m_ArrayDeque.m_Version)
                throw new InvalidOperationException("Enum Failed Version!");
            m_Index = -1;
            m_CurrentElement = default!;
        }
    }
    
    #endregion
    
    #region AddMembers

    /// <summary>
    /// Adds an object to the end of the ArrayDeque.
    /// </summary>
    public void Add(T item) => Enqueue(item);
    
    /// <summary>
    /// Adds an object to the end of the ArrayDeque.
    /// </summary>
    public void AddLast(T item) => Enqueue(item);
    
    /// <summary>
    /// Adds an object to the end of the ArrayDeque.
    /// </summary>
    public void Enqueue(T item)
    {
        if (Count == m_Array.Length) Grow(Count + 1);
        m_Array[m_Tail] = item;
        MoveNext(ref m_Tail);
        ++Count;
        ++m_Version;
    }
    
    /// <summary>
    /// Adds an object to the beginning of the ArrayDeque.
    /// </summary>
    public void AddFirst(T item) => OfferFirst(item);
    
    /// <summary>
    /// Adds an object to the beginning of the ArrayDeque.
    /// </summary>
    public void OfferFirst(T item)
    {
        if (Count == m_Array.Length) Grow(Count + 1);
        MoveFront(ref m_Head);
        m_Array[m_Head] = item;
        ++Count;
        ++m_Version;
    }
    
    #endregion
    
    #region RemoveMembers

    /// <summary>
    /// Removes the object at the beginning of the ArrayDeque.
    /// Returns false if the collection is empty.
    /// </summary>
    public void Remove() => Dequeue();
    
    /// <summary>
    /// Removes the object at the beginning of the ArrayDeque.
    /// Returns false if the collection is empty.
    /// </summary>
    public void RemoveFirst() => Dequeue();

    /// <summary>
    /// Removes and returns the object at the beginning of the ArrayDeque.
    /// </summary>
    public T PollFirst() => Dequeue();
    
    /// <summary>
    /// Removes and returns the object at the beginning of the ArrayDeque.
    /// </summary>
    public T Dequeue()
    {
        if (Count == 0) ThrowForEmptyQueue();
        T obj = m_Array[m_Head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            m_Array[m_Head] = default!;
        MoveNext(ref m_Head);
        --Count;
        ++m_Version;
        return obj;
    }
    
    /// <summary>
    /// Removes the object at the end of the ArrayDeque.
    /// Returns false if the collection is empty.
    /// </summary>
    public void RemoveLast() => PollLast();

    /// <summary>
    /// Removes and returns the object at the end of the ArrayDeque.
    /// </summary>
    public T PollLast()
    {
        if (Count == 0) ThrowForEmptyQueue();
        T obj = m_Array[m_Tail];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            m_Array[m_Tail] = default!;
        MoveFront(ref m_Tail);
        --Count;
        ++m_Version;
        return obj;
    }

    #endregion

    #region QueueMembers

    /// <summary>
    /// Gets the total numbers of elements the internal data structure can hold without resizing.
    /// </summary>
    public int Capacity => m_Array.Length;

    /// <summary>
    /// Returns the object at the beginning of the collection.
    /// </summary>
    public T? First() => Count == 0 ? default : m_Array[m_Head];

    /// <summary>
    /// Returns the object at the beginning of the collection without removing it.
    /// </summary>
    public T PeekFirst() => Peek();
    
    /// <summary>
    /// Returns the object at the beginning of the collection without removing it.
    /// </summary>
    public T Peek()
    {
        if (Count == 0) ThrowForEmptyQueue();
        return m_Array[m_Head];
    }
    
    /// <summary>
    /// Returns a value that indicates whether there is an object at the beginning of the collection,
    /// and if one is present, copies it to the <paramref name="result" /> parameter.
    /// The object is not removed from the collection.
    /// </summary>
    public bool TryPeek([MaybeNullWhen(false)] out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = m_Array[m_Head];
        return true;
    }
    
    /// <summary>
    /// Removes the object at the beginning of the collection,
    /// and copies it to the <paramref name="result" /> parameter.
    /// </summary>
    public bool TryDequeue([MaybeNullWhen(false)] out T result)
    {
        int head = m_Head;
        T[] array = m_Array;
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = array[head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            array[head] = default!;
        MoveNext(ref m_Head);
        --Count;
        ++m_Version;
        return true;
    }
    
    /// <summary>
    /// Removes the object at the beginning of the collection,
    /// and copies it to the <paramref name="result" /> parameter.
    /// </summary>
    public bool TryPollFirst([MaybeNullWhen(false)] out T result)
    {
        int head = m_Head;
        T[] array = m_Array;
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = array[head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            array[head] = default!;
        MoveNext(ref m_Head);
        --Count;
        ++m_Version;
        return true;
    }
    
    /// <summary>
    /// Returns the object at the end of the collection.
    /// </summary>
    public T? Last() => Count == 0 ? default : m_Array[m_Tail];

    /// <summary>
    /// Returns the object at the end of the collection without removing it.
    /// </summary>
    public T PeekLast()
    {
        if (Count == 0) ThrowForEmptyQueue();
        return m_Array[m_Tail];
    }
    
    /// <summary>
    /// Returns a value that indicates whether there is an object at the end of the collection,
    /// and if one is present, copies it to the <paramref name="result" /> parameter.
    /// The object is not removed from the collection.
    /// </summary>
    public bool TryPeekLast([MaybeNullWhen(false)] out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = m_Array[m_Tail];
        return true;
    }
    
    /// <summary>
    /// Removes the object at the end of the collection,
    /// and copies it to the <paramref name="result" /> parameter.
    /// </summary>
    public bool TryPollLast([MaybeNullWhen(false)] out T result)
    {
        int tail = m_Tail;
        T[] array = m_Array;
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = array[tail];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            array[tail] = default!;
        MoveFront(ref m_Tail);
        --Count;
        ++m_Version;
        return true;
    }

    private void SetCapacity(int capacity)
    {
        T[] destinationArray = new T[capacity];
        if (Count > 0)
        {
            if (m_Head < m_Tail)
            {
                Array.Copy(m_Array, m_Head, destinationArray, 0, Count);
            }
            else
            {
                Array.Copy(m_Array, m_Head, destinationArray, 0, m_Array.Length - m_Head);
                Array.Copy(m_Array, 0, destinationArray, m_Array.Length - m_Head, m_Tail);
            }
        }

        m_Array = destinationArray;
        m_Head = 0;
        m_Tail = Count == capacity ? 0 : Count;
        ++m_Version;
    }
    
    #endregion
    
    private void MoveFront(ref int index)
    {
        int num = index - 1;
        if (num == -1) num = m_Array.Length - 1;
        index = num;
    }
    
    private void MoveNext(ref int index)
    {
        int num = index + 1;
        if (num == m_Array.Length) num = 0;
        index = num;
    }

    private static void ThrowForEmptyQueue() => throw new InvalidOperationException("Deque is empty!");

    /// <summary>
    /// Sets the capacity to the actual number of elements in the collection,
    /// if that number is less than 90 percent of current capacity.
    /// </summary>
    public void TrimExcess()
    {
        if (Count >= (int)(m_Array.Length * 0.9)) return;
        SetCapacity(Count);
    }

    /// <summary>
    /// Sets the capacity of a ArrayDeque object to the specified number of entries.
    /// </summary>
    public void TrimExcess(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, Count, nameof(capacity));
        if (capacity == m_Array.Length) return;
        SetCapacity(capacity);
    }

    /// <summary>
    /// Ensures that the capacity of this queue is at least the specified <paramref name="capacity" />.
    /// If the current capacity is less than <paramref name="capacity" />,
    /// it is increased to at least the specified <paramref name="capacity" />.
    /// </summary>
    public int EnsureCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        if (m_Array.Length < capacity) Grow(capacity);
        return m_Array.Length;
    }

    private void Grow(int capacity)
    {
        int val1 = 2 * m_Array.Length;
        if ((uint)val1 > 2147483591U) val1 = 2147483591;
        int capacity1 = Math.Max(val1, m_Array.Length + 4);
        if (capacity1 < capacity) capacity1 = capacity;
        SetCapacity(capacity1);
    }
}