using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace MiHoMiao.Core.Numerics.GameDesign;

/// <summary>
/// 模块控制器
/// </summary>
public class ModuleController<TModule> : ICollection<TModule>, ICollection where TModule : class, IModule
{
    
    #region Fields
    private Dictionary<Type, TModule> EntityModules { get; } = [];
    
    /// <summary>
    /// 根据 type 获取这个对象的一个 Module
    /// </summary>
    public TModule this[Type type] => EntityModules[type];

    /// <summary>
    /// 尝试添加一个签名为 T 的模块,
    /// 返回是否添加成功.
    /// </summary>
    public bool AddModule<T>(T module) where T : TModule
    {
        bool result = EntityModules.TryAdd(typeof(T), module);
        if (result) module.ModuleInitialize();
        return result;
    }
    
    /// <summary>
    /// 根据 type 获取这个对象的一个 EntityModule.
    /// 添加后, 会执行 callBack 回调.
    /// </summary>
    public bool AddModule<T>(T module, Action<T> callBack) where T : TModule
    {
        bool success = AddModule(module);
        callBack(module);
        return success;
    }
    
    /// <summary>
    /// 尝试添加一个签名为 T 的模块,
    /// 返回是否添加成功.
    /// </summary>
    public bool AddModule(Type type, TModule module)
    {
        bool result = EntityModules.TryAdd(type, module);
        if (result) module.ModuleInitialize();
        return result;
    }
    
    /// <summary>
    /// 尝试添加一系列模块,
    /// 返回添加成功的数量.
    /// </summary>
    public int AddModules(IEnumerable<TModule> modules) => modules.Count(module => AddModule(module.GetType(), module));

    /// <summary>
    /// 强制添加一个签名为 T 的模块.
    /// 如果相同签名的模块已经存在, 会被新的模块覆盖.
    /// </summary>
    public void OverrideModule<T>(T module) where T : TModule
    {
        EntityModules[typeof(T)] = module;
        module.ModuleInitialize();
    }

    /// <summary>
    /// 根据 T 获取这个对象的一个 Module
    /// </summary>
    [Pure]
    public T? GetModule<T>() where T : class, TModule 
        => EntityModules.TryGetValue(typeof(T), out TModule? module) ? module as T : null;
    
    /// <summary>
    /// 返回找到的首个满足类型 T 的 Module.
    /// </summary>
    [Pure]
    public T? FirstOfType<T>() where T : class, TModule => EntityModules.Values.OfType<T>().FirstOrDefault();

    /// <summary>
    /// 尝试根据 type 获取这个对象的一个 Module
    /// </summary>
    public bool TryGetModule(Type type, [MaybeNullWhen(false)] out TModule entityModule)
        => EntityModules.TryGetValue(type, out entityModule);
    
    /// <summary>
    /// 尝试根据 T 获取这个对象的一个 Module
    /// </summary>
    public bool TryGetModule<T>(out T? entityModule) where T : class, TModule
    {
        bool result = TryGetModule(typeof(T), out TModule? outValue);
        entityModule = outValue as T;
        return result;
    }
    #endregion

    #region ICollections
    public IEnumerator<TModule> GetEnumerator() => EntityModules.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(TModule item) => AddModule(item.GetType(), item);

    public void Clear() => EntityModules.Clear();

    public bool Contains(TModule item) => EntityModules.ContainsKey(item.GetType());

    public void CopyTo(TModule[] array, int arrayIndex) => EntityModules.Values.CopyTo(array, arrayIndex);

    public bool Remove(TModule item) => EntityModules.Remove(item.GetType());

    public void CopyTo(Array array, int index) => CopyTo((TModule[])array, index);

    public int Count => EntityModules.Count;
    public bool IsSynchronized => false;
    public object SyncRoot => this;
    public bool IsReadOnly => false;

    #endregion
    
}

/// <summary>
/// 用于 <see cref="T:ModuleController`1"/> 的接口.
/// </summary>
public interface IModule
{
    void ModuleInitialize();
}