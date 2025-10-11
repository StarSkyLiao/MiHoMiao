using System.Globalization;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.xUnit.Core.Numerics.Values;

public class Fixed64Tests
{
    #region 构造 & 溢出

    [Theory]
    [InlineData(123)]
    [InlineData(-456)]
    public void Ctor_Int32_Works(int v)
    {
        Fixed64 f = new Fixed64(v);
        Assert.Equal(v, f.AsInt32());
    }

    [Theory]
    [InlineData(123.456d)]
    [InlineData(-78.9d)]
    public void Ctor_Double_Works(double v)
    {
        Fixed64 f = new Fixed64(v);
        Assert.Equal(v, f.AsFloat64(), 8);
    }

    [Fact]
    public void Ctor_Long_Overflow_Throws()
    {
        long big = Fixed64.MaxValue.AsInt64() + 1;
        Assert.Throws<OverflowException>(() => new Fixed64(big));
    }

    [Fact]
    public void Ctor_Double_Overflow_Throws()
    {
        double big = long.MaxValue;
        Assert.Throws<OverflowException>(() => new Fixed64(big));
    }

    #endregion

    #region 构造 & 溢出 ---- 追加用例
    [Theory]
    [InlineData(0)]                       // 0
    [InlineData(int.MinValue)]            // 最小 32 位
    [InlineData(int.MaxValue)]
    public void Ctor_Int32_Edge(int v)
    {
        Fixed64 f = new Fixed64(v);
        Assert.Equal(v, f.AsInt32());
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(double.Epsilon)]          // 最小正浮点数
    [InlineData(-double.Epsilon)]
    [InlineData(1e-8)]                    // 低于分辨率
    [InlineData(-1e-8)]
    [InlineData(9223372036854775807.0)]   // long.MaxValue
    [InlineData(-9223372036854775808.0)]  // long.MinValue
    public void Ctor_Double_Edge(double v)
    {
        if (v > Fixed64.MaxValue.AsFloat64() || v < Fixed64.MinValue.AsFloat64())
            Assert.Throws<OverflowException>(() => new Fixed64(v));
        else
        {
            Fixed64 f = new Fixed64(v);
            Assert.Equal(v, f.AsFloat64(), 8);
        }
    }

    [Fact]
    public void Ctor_Long_NegativeOverflow()
    {
        long small = Fixed64.MinValue.AsInt64() - 1;
        Assert.Throws<OverflowException>(() => new Fixed64(small));
    }
    #endregion
    
    #region 转换

    [Theory]
    [InlineData(42)]
    [InlineData(-3.14)]
    public void Implicit_From_Double(double v)
    {
        Fixed64 f = v; // implicit
        Assert.Equal(v, f.AsFloat64(), 8);
    }

    [Theory]
    [InlineData(42.12345678)]
    public void Explicit_To_Decimal_RoundTrip(decimal v)
    {
        Fixed64 f = (Fixed64)v;
        decimal back = f;
        Assert.Equal(v, back);
    }

    #endregion

    #region 转换 ---- 追加用例
    [Theory]
    [InlineData(-0.0)]
    [InlineData(-3.14159265)]
    [InlineData(-1e-10)]
    public void Implicit_From_Double_Negative(double v)
    {
        Fixed64 f = v;
        Assert.Equal(v, f.AsFloat64(), 8);
    }
    
    #endregion
    
    #region 运算符

    [Fact]
    public void Add_Sub_Mul_Div()
    {
        Fixed64 a = 3.5;
        Fixed64 b = 2.0;
        Assert.Equal(5.5, (a + b).AsFloat64(), 8);
        Assert.Equal(1.5, (a - b).AsFloat64(), 8);
        Assert.Equal(7.0, (a * b).AsFloat64(), 8);
        Assert.Equal(1.75, (a / b).AsFloat64(), 8);
    }
    
    [Fact]
    public void Big_Add_Sub_Mul_Div()
    {
        decimal decimalA = 92133710360.54775807M;
        decimal decimalB = 12312532;
        Fixed64 a = new Fixed64(decimalA);
        Fixed64 b = new Fixed64(decimalB);
        Assert.Equal((double)(decimalA + decimalB), (a + b).AsFloat64(), 8);
        Assert.Equal((double)(decimalA - decimalB), (a - b).AsFloat64(), 8);
        Assert.Equal(Math.Round(decimalA / decimalB, 8, MidpointRounding.ToZero) , (a / b).AsDecimal(), 8);
    }

    [Fact]
    public void Increment_Decrement()
    {
        Fixed64 f = 10;
        f++;
        Assert.Equal(11, f.AsInt32());
        f--;
        Assert.Equal(10, f.AsInt32());
    }

    [Fact]
    public void Unary_Neg()
    {
        Fixed64 f = 42.5;
        Assert.Equal(-42.5, (-f).AsFloat64(), 8);
    }

    #endregion

    #region 运算符 ---- 追加用例
    [Fact]
    public void Add_Sub_Mul_Div_NegativeZero()
    {
        Fixed64 a = 0.0;
        Fixed64 b = -0.0;
        Assert.Equal(0.0, (a + b).AsFloat64(), 8);
        Assert.Equal(0.0, (a - b).AsFloat64(), 8);
        Assert.Equal(-0.0, (b - a).AsFloat64(), 8);
    }

    [Theory]
    [InlineData(-3.5, -2.0, -5.5)]   // 负 + 负
    [InlineData(-3.5,  2.0, -1.5)]   // 负 + 正
    [InlineData( 3.5, -2.0,  1.5)]   // 正 + 负
    public void Add_Sub_Basic(double x, double y, double expectedAdd)
    {
        Fixed64 fx = x, fy = y;
        Assert.Equal(expectedAdd, (fx + fy).AsFloat64(), 8);
        Assert.Equal(expectedAdd, (fy + fx).AsFloat64(), 8); // 交换律
        Assert.Equal(x - y, (fx - fy).AsFloat64(), 8);
    }

    [Theory]
    [InlineData(0.1)]
    [InlineData(-0.1)]
    [InlineData(0.00000001)]
    [InlineData(-0.00000001)]
    public void Mul_Div_SmallNumbers(double v)
    {
        Fixed64 f = v;
        Assert.Equal(v * v, (f * f).AsFloat64(), 8);
        Assert.Equal(1.0, (f / f).AsFloat64(), 8);
    }

    [Fact]
    public void Div_ByZero_Throws()
    {
        Fixed64 a = 1.0;
        Fixed64 zero = 0.0;
        Assert.Throws<DivideByZeroException>(() => a / zero);
    }

    #endregion
    
    #region 比较

    [Theory]
    [InlineData(1, 2, -1)]
    [InlineData(2, 1, 1)]
    [InlineData(5, 5, 0)]
    public void CompareTo_Works(int x, int y, int expected)
    {
        Fixed64 fx = x;
        Fixed64 fy = y;
        Assert.Equal(expected, fx.CompareTo(fy));
    }

    [Fact]
    public void Equality_Operators()
    {
        Fixed64 f1 = 3.2;
        Fixed64 f2 = 3.2;
        Fixed64 f3 = 3.3;
        Assert.True(f1 == f2);
        Assert.False(f1 == f3);
        Assert.True(f1 != f3);
        Assert.True(f1 <= f2);
        Assert.True(f1 >= f2);
        Assert.True(f1 < f3);
        Assert.True(f3 > f1);
    }

    #endregion

    #region 比较 ---- 追加用例

    [Fact]
    public void Equality_NegativeZero()
    {
        Fixed64 nzero = -0.0;
        Fixed64 pzero = 0.0;
        Assert.True(nzero == pzero);
        Assert.Equal(0, nzero.CompareTo(pzero));
    }
    #endregion
    
    #region 边界 & 常量

    [Fact]
    public void MinValue_MaxValue_Work()
    {
        Assert.True(Fixed64.MinValue < Fixed64.MaxValue);
        Assert.Equal(long.MinValue / 1_0000_0000L, Fixed64.MinValue.AsInt64());
        Assert.Equal(long.MaxValue / 1_0000_0000L, Fixed64.MaxValue.AsInt64());
    }

    [Fact]
    public void Constants()
    {
        Assert.Equal(0, Fixed64.Zero.AsInt32());
        Assert.Equal(1, Fixed64.One.AsInt32());
        Assert.Equal(-1, Fixed64.NegativeOne.AsInt32());
    }

    #endregion

    #region 边界 & 常量 ---- 追加用例

    [Fact]
    public void NegativeOne_Abs_IsOne()
    {
        Assert.Equal(Fixed64.One, Fixed64.Abs(Fixed64.NegativeOne));
    }
    #endregion
    
    #region 格式化 & 解析

    [Theory]
    [InlineData("123.456789")]
    [InlineData("-0.00000001")]
    public void Parse_ToString_RoundTrip(string txt)
    {
        Fixed64 f = Fixed64.Parse(txt, CultureInfo.InvariantCulture);
        string back = f.ToString("G", CultureInfo.InvariantCulture);
        Assert.Equal(txt, back);
    }

    [Fact]
    public void TryParse_Fails_On_Garbage()
    {
        bool ok = Fixed64.TryParse("abc", CultureInfo.InvariantCulture, out var f);
        Assert.False(ok);
        Assert.Equal(default, f);
    }

    #endregion

    #region 格式化 & 解析 ---- 追加用例

    [Theory]
    [InlineData("abc")]
    [InlineData("123.456.789")]
    [InlineData("--123")]
    [InlineData("")]
    public void TryParse_Fails_OnGarbage(string garbage)
    {
        Assert.False(Fixed64.TryParse(garbage, CultureInfo.InvariantCulture, out _));
    }
    #endregion
    
    #region NumberInterface 辅助

    [Theory]
    [InlineData(4, true)]
    [InlineData(3, false)]
    public void IsEvenInteger(int v, bool expected)
    {
        Fixed64 f = v;
        Assert.Equal(expected, Fixed64.IsEvenInteger(f));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(0.00000001, false)]
    public void IsInteger(double v, bool expected)
    {
        Fixed64 f = v;
        Assert.Equal(expected, Fixed64.IsInteger(f));
    }

    [Fact]
    public void Abs()
    {
        Fixed64 f = -123.456;
        Assert.Equal(123.456, Fixed64.Abs(f).AsFloat64(), 8);
    }

    #endregion
    
    #region NumberInterface 辅助 ---- 追加用例
    [Theory]
    [InlineData(-4, true)]
    [InlineData(-3, false)]
    [InlineData(0, true)]
    public void IsEvenInteger_Negative(int v, bool expected)
    {
        Fixed64 f = v;
        Assert.Equal(expected, Fixed64.IsEvenInteger(f));
    }

    [Theory]
    [InlineData(-1.0, true)]
    [InlineData(-1.00000001, false)]
    [InlineData(-0.99999999, false)]
    public void IsInteger_Negative(decimal v, bool expected)
    {
        Fixed64 f = new Fixed64(v);
        Assert.Equal(expected, Fixed64.IsInteger(f));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-123.456)]
    public void Abs_Negative(double v)
    {
        Fixed64 f = v;
        Assert.True(Fixed64.Abs(f) >= Fixed64.Zero);
        if (v != 0.0)
            Assert.Equal(Math.Abs(v), Fixed64.Abs(f).AsFloat64(), 8);
    }
    #endregion
    
}