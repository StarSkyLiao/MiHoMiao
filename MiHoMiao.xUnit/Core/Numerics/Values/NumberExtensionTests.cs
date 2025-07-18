using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.xUnit.Core.Numerics.Values;

public class NumberExtensionTests
{
    [Theory]
    [InlineData(-5, 5)]
    [InlineData(5, 5)]
    [InlineData(0, 0)]
    public void Abs_Int_ReturnsAbsoluteValue(int value, int expected)
    {
        // Act
        int result = value.Abs();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-3.14, 3.14)]
    [InlineData(3.14, 3.14)]
    [InlineData(0.0, 0.0)]
    public void Abs_Double_ReturnsAbsoluteValue(double value, double expected)
    {
        // Act
        double result = value.Abs();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-5, -1)]
    [InlineData(0, 0)]
    [InlineData(5, 1)]
    public void Sign_Int_ReturnsCorrectSign(int value, int expected)
    {
        // Act
        int result = value.Sign();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-3.14, -1)]
    [InlineData(0.0, 0)]
    [InlineData(3.14, 1)]
    public void Sign_Double_ReturnsCorrectSign(double value, int expected)
    {
        // Act
        int result = value.Sign();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2, 3, 8)]
    [InlineData(5, 2, 25)]
    [InlineData(3, 0, 1)]
    [InlineData(-2, 3, -8)]
    public void Pow_Int_ReturnsCorrectPower(int value, int power, int expected)
    {
        // Act
        int result = value.Pow(power);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2.0, 3, 8.0)]
    [InlineData(5.0, 2, 25.0)]
    [InlineData(3.0, 0, 1.0)]
    public void Pow_Double_ReturnsCorrectPower(double value, int power, double expected)
    {
        // Act
        double result = value.Pow(power);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 10, 10)]
    [InlineData(15, 10, 15)]
    [InlineData(8, 10, 10)]
    [InlineData(-5, 0, 0)]
    public void Min_Int_RestrictsToMinimum(int value, int min, int expected)
    {
        // Act
        int result = value.Min(min);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5.0, 10.0, 10.0)]
    [InlineData(15.0, 10.0, 15.0)]
    [InlineData(8.0, 10.0, 10.0)]
    public void Min_Double_RestrictsToMinimum(double value, double min, double expected)
    {
        // Act
        double result = value.Min(min);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(15, 10, 10)]
    [InlineData(5, 10, 5)]
    [InlineData(12, 10, 10)]
    [InlineData(-5, 0, -5)]
    public void Max_Int_RestrictsToMaximum(int value, int max, int expected)
    {
        // Act
        int result = value.Max(max);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(15.0, 10.0, 10.0)]
    [InlineData(5.0, 10.0, 5.0)]
    [InlineData(12.0, 10.0, 10.0)]
    public void Max_Double_RestrictsToMaximum(double value, double max, double expected)
    {
        // Act
        double result = value.Max(max);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    public void Clamp01_Int_RestrictsToZeroOneRange(int value, int expected)
    {
        // Act
        int result = value.Clamp01();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-0.5, 0.0)]
    [InlineData(0.5, 0.5)]
    [InlineData(1.5, 1.0)]
    [InlineData(0.0, 0.0)]
    public void Clamp01_Double_RestrictsToZeroOneRange(double value, double expected)
    {
        // Act
        double result = value.Clamp01();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(15, 0, 10, 10)]
    [InlineData(-5, 0, 10, 0)]
    [InlineData(5, 0, 10, 5)]
    [InlineData(12, 0, 10, 10)]
    public void Clamp_Int_RestrictsToRange(int value, int min, int max, int expected)
    {
        // Act
        int result = value.Clamp(min, max);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(15.0, 0.0, 10.0, 10.0)]
    [InlineData(-5.0, 0.0, 10.0, 0.0)]
    [InlineData(5.0, 0.0, 10.0, 5.0)]
    [InlineData(12.0, 0.0, 10.0, 10.0)]
    public void Clamp_Double_RestrictsToRange(double value, double min, double max, double expected)
    {
        // Act
        double result = value.Clamp(min, max);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(15, 0, 10, 5)]
    [InlineData(-5, 0, 10, 5)]
    [InlineData(25, 0, 10, 5)]
    [InlineData(5, 0, 10, 5)]
    [InlineData(10, 0, 10, 0)]
    public void CircleClamp_Int_WrapsToRange(int value, int leftInclude, int rightExclude, int expected)
    {
        // Act
        int result = value.CircleClamp(leftInclude, rightExclude);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(15.0, 0.0, 10.0, 5.0)]
    [InlineData(-5.0, 0.0, 10.0, 5.0)]
    [InlineData(25.0, 0.0, 10.0, 5.0)]
    [InlineData(5.0, 0.0, 10.0, 5.0)]
    [InlineData(10.0, 0.0, 10.0, 0.0)]
    public void CircleClamp_Double_WrapsToRange(double value, double leftInclude, double rightExclude, double expected)
    {
        // Act
        double result = value.CircleClamp(leftInclude, rightExclude);

        // Assert
        Assert.Equal(expected, result);
    }
}