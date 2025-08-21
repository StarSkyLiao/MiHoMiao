namespace MiHoMiao.Migxn.Runtime;

public class MigxnSymbols<T>(MigxnSymbols<T>? parentTable = null)
{
    private readonly List<Dictionary<string, T>> m_JarfterObjects = [[]];
    
    public MigxnSymbols<T>? ParentTable => parentTable;

    public int Count => m_JarfterObjects.Sum(dic => dic.Count);

    public void DeclareVariable(string name, T @object)
    {
        if (m_JarfterObjects[^1].TryAdd(name, @object)) return;
        throw new NotSupportedException($"Variable {name} is already declared!");
    }

    public void StoreVariable(string name, T @object)
    {
        if (name is "_") m_JarfterObjects[^1]["_"] = @object;
        else
        {
            for (var index = m_JarfterObjects.Count - 1; index >= 0; index--)
            {
                Dictionary<string, T> item = m_JarfterObjects[index];
                if (!item.ContainsKey(name)) continue;
                item[name] = @object;
                return;
            }
        }
        if (ParentTable != null) ParentTable.StoreVariable(name, @object);
        else throw new NotSupportedException($"Variable {name} is not declared!");
    }

    public T LoadVariable(string name)
    {
        for (int index = m_JarfterObjects.Count - 1; index >= 0; index--)
        {
            Dictionary<string, T> item = m_JarfterObjects[index];
            if (item.TryGetValue(name, out T? value)) return value;
        }
        return ParentTable != null 
            ? ParentTable.LoadVariable(name) 
            : throw new NotSupportedException($"Variable {name} is not declared!");
    }

    public void PushScope() => m_JarfterObjects.Add([]);

    public void PopScope() => m_JarfterObjects.RemoveAt(m_JarfterObjects.Count - 1);
    
}