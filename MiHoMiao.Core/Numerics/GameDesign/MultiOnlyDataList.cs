namespace MiHoMiao.Core.Numerics.GameDesign;

public record MultiOnlyDataList(DataProperty DataProperty, double BaseValue = 1) : AbstractDataList(DataProperty, BaseValue)
{
    
    public void Multiply(double value)
    {
        double oldValue = DataProperty.Value;
        BaseValue *= value;
        NotifyValueChanged(oldValue);
    }
    
    public void Multiply(Func<double> func)
    {
        double oldValue = DataProperty.Value;
        DataList.Add(func);
        NotifyValueChanged(oldValue);
    }
    
    public void Division(double value)
    {
        double oldValue = DataProperty.Value;
        BaseValue /= value;
        NotifyValueChanged(oldValue);
    }
    
    public void Division(Func<double> func)
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
            foreach (Func<double> item in DataList) result *= item();
            return result;
        }
    }
    
}