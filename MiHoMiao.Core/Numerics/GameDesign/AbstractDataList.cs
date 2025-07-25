namespace MiHoMiao.Core.Numerics.GameDesign;

public abstract record AbstractDataList(DataProperty DataProperty, double BaseValue)
{
    protected readonly List<Func<double>> DataList = [];

    protected double BaseValue = BaseValue;

    /// <summary>
    /// 返回经过调用链之后的值
    /// </summary>
    public abstract double Value { get; }

    protected void NotifyValueChanged(double oldValue) => DataProperty.TriggerEvent(oldValue, DataProperty.Value);
    
}