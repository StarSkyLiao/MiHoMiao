using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Core;

public class JarfterSymbolTable<T>(JarfterSymbolTable<T>? parentTable = null)
{
    private readonly Dictionary<string, T?> m_JarfterObjects = [];
    
    public JarfterSymbolTable<T>? ParentTable => parentTable;

    public void DeclareVariable(string name, T? jarfterObject) => m_JarfterObjects.Add(name, jarfterObject);

    public void StoreVariable(string name, T? jarfterObject)
    {
        if (m_JarfterObjects.ContainsKey(name)) m_JarfterObjects[name] = jarfterObject;
        else if (ParentTable != null) ParentTable.StoreVariable(name, jarfterObject);
        else throw new VariableNotDeclaredException(name);
    }

    public T? LoadVariable(string name)
    {
        if (m_JarfterObjects.TryGetValue(name, out T? value)) return value;
        return ParentTable != null ? ParentTable.LoadVariable(name) : default;
    }
}