namespace MiHoMiao.Core.Numerics.GameDesign;

/// <summary>
/// 一个存储 double 类型数据的盒子.
/// 访问一个不存在的数据时, 自动创建新项并返回 0.
/// </summary>
public class DataBox<T> where T : notnull
{

    private readonly Dictionary<T, double> m_DataBox = [];

    private readonly Dictionary<T, Action<double>> m_OnSetAction = [];
    
    public bool Contains(T key) => m_DataBox.ContainsKey(key);
    
    public bool TryInsert(T key, double value) => m_DataBox.TryAdd(key, value);
    
    /// <summary>
    /// 获取键 key 对应的值.
    /// 如果不存在, 则自动创建并给予默认值 0.
    /// </summary>
    public double this[T key]
    {
        set
        {
            if (m_OnSetAction.TryGetValue(key, out Action<double>? action)) action(value);
            m_DataBox[key] = value;
        }
        get
        {
            if (m_DataBox.TryGetValue(key, out double item)) return item;
            return m_DataBox[key] = 0;
        }
    }

    /// <summary>
    /// 注册一个赋值时触发的动作.
    /// 为 key 赋值时, 触发 action 动作.
    /// </summary>
    public void RegisterSetter(T key, Action<double> action)
    {
        if (!m_OnSetAction.TryAdd(key, action)) m_OnSetAction[key] += action;
    }
    
}