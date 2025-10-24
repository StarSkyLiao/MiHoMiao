//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

namespace MiHoMiao.Core.Collections.Generic;

/// <summary>
/// A Trie is a special form of an N-fork tree.
/// Each node of the Trie represents a prefix.
/// Each node will have multiple sub-nodes,
/// and the paths to different sub-nodes will have different path.
/// The path leads to a specific object stored by this collection.
/// </summary>
public class Trie<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, (TValue? Value, Trie<TKey, TValue?> Next)> m_Trie =
        new Dictionary<TKey, (TValue?, Trie<TKey, TValue?>)>();

    public TValue? this[params TKey[] keys]
    {
        get => Get_Internal(keys);
        set => Set_Internal(keys, value);
    }
    
    public TValue? this[params ReadOnlySpan<TKey> keys]
    {
        get => Get_Internal(keys);
        set => Set_Internal(keys, value);
    }
    
    /// <summary>
    /// Set the value of path - keys to value.
    /// </summary>
    public void SetValue(TValue? value, params TKey[] keys) => Set_Internal(keys, value);
    
    /// <summary>
    /// Set the value of path - keys to value.
    /// </summary>
    public TValue? GetValue(params TKey[] keys) => Get_Internal(keys);
    
    /// <summary>
    /// Returns whether there is a value that stored in path of keys.
    /// </summary>
    public bool StartsWith(ReadOnlySpan<TKey> keys) => keys.Length switch
    {
        0 => false,
        1 => m_Trie.ContainsKey(keys[0]),
        _ => m_Trie.ContainsKey(keys[0]) && m_Trie[keys[0]].Next.StartsWith(keys[1..])
    };
    
    /// <summary>
    /// Return the first element that satisfies the key with the specified value.
    /// </summary>
    public TValue? SearchFirst(TKey key)
    {
        if (m_Trie.TryGetValue(key, out var value)) return value.Value;
        foreach ((TValue? _, Trie<TKey, TValue?> next) in m_Trie.Values)
        {
            TValue? result = next.SearchFirst(key);
            if (result != null) return result;
        }
        return default;
    }

    private void Set_Internal(ReadOnlySpan<TKey> keys, TValue? value)
    {
        switch (keys.Length)
        {
            case > 1:
                m_Trie.TryAdd(keys[0], (default, new Trie<TKey, TValue>())!);
                m_Trie[keys[0]].Next.Set_Internal(keys[1..], value);
                break;
            case 1:
                m_Trie.TryAdd(keys[0], (default, new Trie<TKey, TValue>())!);
                m_Trie[keys[0]] = (value, m_Trie[keys[0]].Next);
                break;
        }
    }
    
    private TValue? Get_Internal(ReadOnlySpan<TKey> keys)
    {
        if (keys.Length == 0) return default;
        (TValue? Value, Trie<TKey, TValue?> Next) value;
        if (keys.Length == 1) return m_Trie.TryGetValue(keys[0], out value) ? value.Value : default;
        if (!m_Trie.TryGetValue(keys[0], out value)) return default;
        return value.Next.Get_Internal(keys[1..]) ?? default;
    }
    
}