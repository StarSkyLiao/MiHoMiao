namespace MiHoMiao.Migxn.Collections;

public class TextTrie<TValue>
{
    private readonly (TValue? Value, TextTrie<TValue>? Next)[] m_Trie = new (TValue? Value, TextTrie<TValue>? Next)[sbyte.MaxValue];
    
    public TValue? this[params ReadOnlySpan<char> keys]
    {
        get => Get_Internal(keys);
        set => Set_Internal(keys, value);
    }
    
    public TextTrie<TValue>? LoadSubTrie(char key) => key >= sbyte.MaxValue ? null : m_Trie[key].Next;

    private TValue? Get_Internal(ReadOnlySpan<char> keys)
    {
        if (keys.Length == 0 || keys[0] >= sbyte.MaxValue) return default;
        (TValue? Value, TextTrie<TValue>? Next) value = m_Trie[keys[0]];
        if (keys.Length == 1) return value.Value;
        return value.Next != null ? value.Next.Get_Internal(keys[1..]) : default;
    }
    
    private void Set_Internal(ReadOnlySpan<char> keys, TValue? value)
    {
        ref (TValue? Value, TextTrie<TValue>? Next) pair = ref m_Trie[keys[0]];
        if (keys.Length == 1) pair.Value = value;
        else if (keys.Length > 1) (pair.Next ??= new TextTrie<TValue>()).Set_Internal(keys[1..], value);
    }
    
}