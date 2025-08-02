using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Core.Numerics.GameDesign;

/// <summary>
/// 一个代表了数据属性的值.
/// 用于游戏中角色的攻击力, 防御力等属性.
/// </summary>
public class DataProperty(double baseValue, double rate = 1)
{
    /// <summary>
    /// 属性的额外值
    /// </summary>
    [field: AllowNull, MaybeNull]
    public AddOnlyDataList ExtraValue => field ??= new AddOnlyDataList(this);

    /// <summary>
    /// 属性的增益值
    /// </summary>
    [field: AllowNull, MaybeNull]
    public AddOnlyDataList BuffValue => field ??= new AddOnlyDataList(this);

    /// <summary>
    /// 属性的倍率
    /// </summary>
    [field: AllowNull, MaybeNull]
    public MultiOnlyDataList MultiValue => field ??= new MultiOnlyDataList(this, rate);

    /// <summary>
    /// 属性的最终值
    /// </summary>
    public double Value => (baseValue * (1 + BuffValue.Value) + ExtraValue.Value) * MultiValue.Value;

    /// <summary>
    /// 在修改属性时, 触发动作, 参数表示修改前后的属性最终值
    /// </summary>
    public event Action<double, double>? OnSetAction;

    public void TriggerEvent(double before, double after) => OnSetAction?.Invoke(before, after);
    
    public override string ToString() =>
        $"{MultiValue.Value:F2} * " +
        $"({baseValue:N0} * (1 + {BuffValue.Value:P1}) + {ExtraValue.Value:N0}) " +
        $"= {Value:F2}";

    public static DataProperty One => new DataProperty(1);
    
    public static DataProperty Zero => new DataProperty(0);

}