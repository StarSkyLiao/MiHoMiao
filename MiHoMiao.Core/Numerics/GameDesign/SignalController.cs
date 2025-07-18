using System.Diagnostics.Contracts;

namespace MiHoMiao.Core.Numerics.GameDesign;

public class SignalController
{
    private Dictionary<Type, Delegate> Signals { get; } = [];

    /// <summary>
    /// 注册一个类型为 T, 名称为 T 类型名的信号.
    /// 如果信号已经被注册过, 则无事发生.
    /// 注意: 该信号为 C# 的委托, 而非 Godot 的信号
    /// </summary>
    public void RegisterSignal<T>() where T : Delegate => Signals.TryAdd(typeof(T), null!);
    
    /// <summary>
    /// 查找类型为 T 的信号并添加一个委托.
    /// 找不到指定信号时会直接返回.
    /// 注意: 该信号为 C# 的委托, 而非 Godot 的信号
    /// </summary>
    public void AddToSignal<T>(T signal) where T : Delegate
    {
        if (!Signals.TryGetValue(typeof(T), out Delegate? value)) return;
        Signals[typeof(T)] = Delegate.Combine(value, signal);
    }
    
    /// <summary>
    /// 查找类型为 T 的信号并添加一个委托.
    /// 找不到指定信号时会直接返回.
    /// 注意: 该信号为 C# 的委托, 而非 Godot 的信号
    /// </summary>
    public void AddToSignal(Type type, Delegate signal)
    {
        if (!Signals.TryGetValue(type, out Delegate? value)) return;
        Signals[type] = Delegate.Combine(value, signal);
    }
    
    /// <summary>
    /// 查找类型为 T 的信号并删除指定的一个委托.
    /// 返回是否成功删除.
    /// 注意: 该信号为 C# 的委托, 而非 Godot 的信号
    /// </summary>
    public void RemoveSignal<T>(T signal) where T : Delegate
    {
        if (!Signals.TryGetValue(typeof(T), out Delegate? value)) return;
        Signals[typeof(T)] = Delegate.Remove(value, signal)!;
    }
    
    /// <summary>
    /// 查找类型为 T 的信号并删除指定的一个委托.
    /// 返回是否成功删除.
    /// 注意: 该信号为 C# 的委托, 而非 Godot 的信号
    /// </summary>
    public void RemoveSignal(Type type, Delegate signal)
    {
        if (!Signals.TryGetValue(type, out Delegate? value)) return;
        Signals[type] = Delegate.Remove(value, signal)!;
    }

    /// <summary>
    /// 根据类型 T, 获取并转化指定的信号.
    /// 当信号不存在或者参数 T 与信号注册的类型不同时, 返回 null.
    /// 注意: 该信号为 C# 的委托, 而非 Godot 的信号
    /// </summary>
    [Pure]
    public T? GetSignal<T>() where T : Delegate
    {
        if (!Signals.TryGetValue(typeof(T), out Delegate? signal)) return null;
        return signal as T;
    }
    
}