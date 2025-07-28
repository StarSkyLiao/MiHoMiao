namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// Custom iterator for <see cref="T:MiHoMiao.Core.Collections.Generic.LruHashSet`1"/>
/// that starts from the most recently added element
/// and supports bidirectional movement with version checking.
/// </summary>
/// <typeparam name="T">The type of elements in the collection, must be non-null.</typeparam>
public class LruIterator<T> where T : notnull
{
    private readonly LruHashSet<T> m_Collection;
    private LinkedListNode<T>? m_CurrentNode;
    private int m_IteratorVersion;

    /// <summary>
    /// Initializes a new instance of the LruHashSetIterator class.
    /// </summary>
    /// <param name="collection">The LruHashSet to iterate over.</param>
    public LruIterator(LruHashSet<T> collection)
    {
        m_Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        m_IteratorVersion = collection.Version;
        Reset();
    }

    /// <summary>
    /// Gets the current element in the iteration.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the iterator is not positioned on a valid element.
    /// </exception>
    public T Current
    {
        get
        {
            CheckVersion();
            if (m_CurrentNode == null)
                throw new InvalidOperationException("Iterator is not positioned on a valid element.");
            return m_CurrentNode.Value;
        }
    }

    /// <summary>
    /// Checks if the iterator is positioned at the first (newest) element.
    /// </summary>
    public bool IsAtFirst => CheckVersion() && m_CurrentNode == m_Collection.LinkedList.First;

    /// <summary>
    /// Checks if the iterator is positioned at the last (oldest) element.
    /// </summary>
    public bool IsAtLast => CheckVersion() && m_CurrentNode == m_Collection.LinkedList.Last;

    /// <summary>
    /// Moves the iterator to the previous (newer) element in the collection.
    /// </summary>
    /// <returns>True if the iterator successfully moved to a previous element; false if already at the first element.</returns>
    public bool MoveFront()
    {
        CheckVersion();
        if (m_CurrentNode == null || m_CurrentNode == m_Collection.LinkedList.First)
            return false;

        m_CurrentNode = m_CurrentNode.Previous;
        return true;
    }

    /// <summary>
    /// Moves the iterator to the next (older) element in the collection.
    /// </summary>
    /// <returns>True if the iterator successfully moved to a next element; false if already at the last element.</returns>
    public bool MoveNext()
    {
        CheckVersion();
        if (m_CurrentNode == null || m_CurrentNode == m_Collection.LinkedList.Last)
            return false;

        m_CurrentNode = m_CurrentNode.Next;
        return true;
    }

    /// <summary>
    /// Resets the iterator to the first (newest) element in the collection.
    /// </summary>
    public void Reset()
    {
        m_IteratorVersion = m_Collection.Version;
        m_CurrentNode = m_Collection.LinkedList.First;
    }

    /// <summary>
    /// Checks if the collection has been modified since the iterator was created
    /// and resets to the newest element if the version has changed.
    /// </summary>
    /// <returns>True if the iterator is still valid; false if the collection is empty.</returns>
    private bool CheckVersion()
    {
        if (m_IteratorVersion != m_Collection.Version)
        {
            Reset();
        }
        return m_Collection.LinkedList.First != null;
    }
}