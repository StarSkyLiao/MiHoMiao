using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.xUnit.Core.Numerics.Values;

public class Fixed64MathTests
{

    [Theory]
    [InlineData(42)]
    [InlineData(13214)]
    [InlineData(63245235212)]
    public void Sqrt(double value)
    {
        Assert.Equal(Math.Sqrt(value), new Fixed64(value).Sqrt(), 7);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(1.54)]
    [InlineData(32)]
    public void Pow(double value)
    {
        Assert.Equal(Math.Pow(2, value), new Fixed64(2).Pow(new Fixed64(value)), 7);
        Assert.Equal(Math.Pow(value, 2), new Fixed64(value).Pow(new Fixed64(2)), 8);
    }
    
    [Theory]
    [InlineData(-1.312)]
    [InlineData(-1)]
    [InlineData(-0.312)]
    [InlineData(0)]
    [InlineData(0.312)]
    [InlineData(1)]
    [InlineData(6.221)]
    public void Exp(double value)
    {
        Assert.Equal(Math.Exp(value), new Fixed64(value).Exp(), 4);
    }
    
    [Theory]
    [InlineData(0.5)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(23131231)]
    [InlineData(343534882L)]
    public void Ln(double value)
    {
        Assert.Equal(Math.Log(value), Fixed64Math.Ln(value), 6);
    }
    
    [Theory]
    [InlineData(0.5)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(23131231)]
    [InlineData(343534882L)]
    public void Log2(double value)
    {
        Assert.Equal(Math.Log2(value), Fixed64Math.Log2(value), 6);
    }
    
    
    
    

}