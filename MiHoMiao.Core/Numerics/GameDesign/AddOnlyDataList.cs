namespace MiHoMiao.Core.Numerics.GameDesign;

public record AddOnlyDataList(DataProperty DataProperty, double BaseValue = 0) : AbstractDataList(DataProperty, BaseValue)
{
    public void Add(double value)
    {
        double oldValue = DataProperty.Value;
        BaseValue += value;
        NotifyValueChanged(oldValue);
    }
    
    public void Add(Func<double> func)
    {
        double oldValue = DataProperty.Value;
        DataList.Add(func);
        NotifyValueChanged(oldValue);
    }
    
    public void Subtract(double value)
    {
        double oldValue = DataProperty.Value;
        BaseValue -= value;
        NotifyValueChanged(oldValue);
    }
    
    public void Subtract(Func<double> func)
    {
        double oldValue = DataProperty.Value;
        DataList.Remove(func);
        NotifyValueChanged(oldValue);
    }

    public override double Value
    {
        get
        {
            double result = BaseValue;
            foreach (Func<double> item in DataList) result += item();
            return result;
        }
    }
}