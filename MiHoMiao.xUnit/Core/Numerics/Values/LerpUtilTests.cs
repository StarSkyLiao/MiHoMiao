using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.xUnit.Core.Numerics.Values;

public class LerpUtilTests
{
    private const double Delta = 0.0001; // Tolerance for double comparisons

    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 2)]
    [InlineData(6, 6)]
    [InlineData(15, 15)]
    public void NumberInterpolation(int number, int expected)
    {
        // Assert
        Assert.Equal(expected, NumberExtension.Number<int>((uint)number));
    }
    
    [Theory]
    [InlineData(0, 10, 0, 0)]
    [InlineData(0, 10, 1, 10)]
    [InlineData(0, 10, 0.5, 5)]
    [InlineData(5, 15, 0.2, 7)]
    [InlineData(-10, 10, 0.5, 0)]
    public void Linear_Int_ReturnsLinearInterpolation(int start, int end, double progress, int expected)
    {
        // Act
        double result = LerpUtil.Linear(start, end, progress);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0.0, 10.0, 0.0, 0.0)]
    [InlineData(0.0, 10.0, 1.0, 10.0)]
    [InlineData(0.0, 10.0, 0.5, 5.0)]
    [InlineData(5.0, 15.0, 0.2, 7.0)]
    [InlineData(-10.0, 10.0, 0.5, 0.0)]
    public void Linear_Double_ReturnsLinearInterpolation(double start, double end, double progress, double expected)
    {
        // Act
        double result = LerpUtil.Linear(start, end, progress);

        // Assert
        Assert.Equal(expected, result, Delta);
    }

    [Theory]
    [InlineData(0, 10, 0, 0)]
    [InlineData(0, 10, 1, 10)]
    [InlineData(0, 10, 0.5, 5)]
    [InlineData(5, 15, 0.2, 6.04)]
    public void Smooth_Int_ReturnsCubicInterpolation(int start, int end, double progress, double expected)
    {
        // Act
        double result = LerpUtil.Smooth(start, end, progress);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0.0, 10.0, 0.0, 0.0)]
    [InlineData(0.0, 10.0, 1.0, 10.0)]
    [InlineData(0.0, 10.0, 0.5, 5.0)]
    [InlineData(5.0, 15.0, 0.2, 6.04)]
    public void Smooth_Double_ReturnsCubicInterpolation(double start, double end, double progress, double expected)
    {
        // Act
        double result = LerpUtil.Smooth(start, end, progress);

        // Assert
        Assert.Equal(expected, result, Delta);
    }

    [Theory]
    [InlineData(0, 10, 0, 0)]
    [InlineData(0, 10, 1, 10)]
    [InlineData(0, 10, 0.5, 5)]
    [InlineData(5, 15, 0.2, 5.5792)]
    public void Quintic_Int_ReturnsQuinticInterpolation(int start, int end, double progress, double expected)
    {
        // Act
        double result = LerpUtil.Quintic(start, end, progress);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0.0, 10.0, 0.0, 0.0)]
    [InlineData(0.0, 10.0, 1.0, 10.0)]
    [InlineData(0.0, 10.0, 0.5, 5.0)]
    [InlineData(5.0, 15.0, 0.2, 5.5792)]
    public void Quintic_Double_ReturnsQuinticInterpolation(double start, double end, double progress, double expected)
    {
        // Act
        double result = LerpUtil.Quintic(start, end, progress);

        // Assert
        Assert.Equal(expected, result, Delta);
    }
}