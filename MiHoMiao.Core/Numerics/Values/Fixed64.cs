using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MiHoMiao.Core.Numerics.Values;

/// <summary>
/// 一个 64-bit 定点数, 能够精确地表示 8 位十进制小数.
/// 数值范围大约为 -92233720368.54776d ~ 92233720368.54776d, 数量级为 1e10(九百亿).
/// 发生溢出时, 不会进行过多检查, 因此需要调用者来处理溢出情况.
/// 用于某些对小鼠的精度有较高要求的场合.
/// </summary>
public readonly struct Fixed64 : INumber<Fixed64>, ISignedNumber<Fixed64>, IMinMaxValue<Fixed64>
{
    internal const long Scale = 1_0000_0000;
    private const long MaxLong = long.MaxValue / Scale;
    private const long MinLong = long.MinValue / Scale;
    private const double MaxFloat = (double)long.MaxValue / Scale;
    private const double MinFloat = (double)long.MinValue / Scale;
    private const decimal MaxDecimal = (decimal)long.MaxValue / Scale;
    private const decimal MinDecimal = (decimal)long.MinValue / Scale;
    
    private const ulong MulFactor = ((1UL << 63) / Scale) << 1;
    private const ulong MulRemain = ((1UL << 63) % Scale) << 1;

    /// <summary>
    /// 实际数值的 1e8 倍
    /// </summary>
    internal readonly long InternalValue;

    #region 构造
    public Fixed64(int value) => InternalValue = value * Scale;
    
    public Fixed64(long value)
    {
        if (value is > MaxLong or < MinLong) ThrowOverflow();
        InternalValue = value * Scale;
    }
    
    public Fixed64(float value) : this((double)value)
    {
    }
    
    public Fixed64(double value) 
    {
        if (value is > MaxFloat or < MinFloat) ThrowOverflow();
        InternalValue = (long)(value * Scale);
    }
    
    public Fixed64(decimal value) 
    {
        if (value is > MaxDecimal or < MinDecimal) ThrowOverflow();
        InternalValue = (long)(value * Scale);
    }
        
    // ReSharper disable once UnusedParameter.Local
    internal Fixed64(long value, bool _) => InternalValue = value;
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowOverflow() => throw new OverflowException("Fixed64 溢出");
    
    #endregion 构造
    
    #region NumberValue

    public int AsInt32() => (int)(InternalValue / Scale);
    
    public long AsInt64() => InternalValue / Scale;
    
    public float AsFloat32() => (float)InternalValue / Scale;
    
    public double AsFloat64() => (double)InternalValue / Scale;
    
    public decimal AsDecimal() => (decimal)InternalValue / Scale;
    
    #endregion NumberValue
    
    #region Math

    public static Fixed64 One => new Fixed64(Scale, true);
    public static Fixed64 Zero => new Fixed64(0, true);
    public static Fixed64 NegativeOne => new Fixed64(-Scale, true);
    internal const long PiValue = (long)(Math.PI * Scale);
    public static Fixed64 Pi => new Fixed64(314159265, true);
    public static Fixed64 E => new Fixed64(271828183, true);
    public static Fixed64 Ln2 => new Fixed64(69314718, true);
    public static Fixed64 Ln10 => new Fixed64(230258509, true);
    public static Fixed64 Ln2Rate => new Fixed64(144269504, true);
    public static Fixed64 MaxValue => new Fixed64(long.MaxValue, true);
    public static Fixed64 MinValue => new Fixed64(long.MinValue, true);
    public static int Radix => 10;
    
    public static Fixed64 AdditiveIdentity => new Fixed64(0);

    public static Fixed64 MultiplicativeIdentity => new Fixed64(1);
    
    public static Fixed64 operator +(Fixed64 value) => value;

    public static Fixed64 operator -(Fixed64 value) => new Fixed64(-value.InternalValue, true);
    
    public static Fixed64 operator ++(Fixed64 value) 
        => new Fixed64(value.InternalValue + Scale, true);
    
    public static Fixed64 operator --(Fixed64 value) 
        => new Fixed64(value.InternalValue - Scale, true);
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static Fixed64 operator +(Fixed64 left, Fixed64 right) 
        => new Fixed64(left.InternalValue + right.InternalValue, true);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static Fixed64 operator -(Fixed64 left, Fixed64 right) 
        => new Fixed64(left.InternalValue - right.InternalValue, true);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static Fixed64 operator *(Fixed64 left, Fixed64 right)
    {
        int sign = left.InternalValue.Sign() * right.InternalValue.Sign();
        ulong upper = Math.BigMul((ulong)left.InternalValue.Abs(), (ulong)right.InternalValue.Abs(), out ulong lower);
        ulong result = MulFactor * upper + (MulRemain * upper + lower) / Scale;
        return new Fixed64(sign * (long)result, true);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static Fixed64 operator /(Fixed64 left, long right)
    {
        return new Fixed64(left.InternalValue / right, true);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static Fixed64 operator /(Fixed64 left, Fixed64 right)
    {
        long rightValue = right.InternalValue;
    
        if (rightValue is <= MaxLong and >= MinLong)
        {
            long div = Math.DivRem(left.InternalValue, rightValue, out long rem);
            return new Fixed64(Scale * div + Scale * rem / rightValue, true);
        }
        else
        {
            if (rightValue < 0) rightValue = -rightValue;
            int sign = left.InternalValue.Sign() * rightValue.Sign();
            ulong upper = Math.BigMul((ulong)left.InternalValue.Abs(), Scale, out ulong lower);
            ulong div = ulong.MaxValue / (ulong)rightValue;
            ulong rem = ulong.MaxValue - div * (ulong)rightValue + 1;
            ulong factor = Math.BigMul(rem, upper, out ulong remFactor);
            return new Fixed64(sign * (long)(div * (upper + factor) + (remFactor + rem * factor + lower) / (ulong)rightValue), true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static Fixed64 operator %(Fixed64 left, Fixed64 right)
        => new Fixed64(left.InternalValue % right.InternalValue, true);
    
    public static Fixed64 Abs(Fixed64 value) => new Fixed64(Math.Abs(value.InternalValue), true);

    #endregion
    
    #region Compare
    
    public bool Equals(Fixed64 other) => other.InternalValue == InternalValue;

    public override bool Equals(object? obj) => obj is Fixed64 other && Equals(other);

    public override int GetHashCode() => InternalValue.GetHashCode();

    public int CompareTo(object? value)
    {
        if (value == null) return 1;
        if (value is not Fixed64 other) throw new ArgumentException("Value Must Be Fixed64!");
        return CompareTo(other);
    }

    public int CompareTo(Fixed64 other) => InternalValue.CompareTo(other.InternalValue);

    public static bool operator >(Fixed64 left, Fixed64 right)
        => left.InternalValue > right.InternalValue;

    public static bool operator >=(Fixed64 left, Fixed64 right)
        => left.InternalValue >= right.InternalValue;

    public static bool operator <(Fixed64 left, Fixed64 right)
        => left.InternalValue < right.InternalValue;
    
    public static bool operator <=(Fixed64 left, Fixed64 right)
        => left.InternalValue <= right.InternalValue;
    
    public static bool operator ==(Fixed64 left, Fixed64 right)
        => left.InternalValue == right.InternalValue;

    public static bool operator !=(Fixed64 left, Fixed64 right)
        => left.InternalValue != right.InternalValue;
    
    #endregion
    
    #region 显式/隐式转化
    public static implicit operator Fixed64(int value)  => new Fixed64(value);
    public static implicit operator Fixed64(long value) => new Fixed64(value);
    public static implicit operator Fixed64(float value) => new Fixed64(value);
    public static implicit operator Fixed64(double value) => new Fixed64(value);
    public static explicit operator Fixed64(decimal value) => new Fixed64(value);
    
    public static explicit operator int(Fixed64 value) => (int)(value.InternalValue / Scale);
    public static explicit operator long(Fixed64 value) => value.InternalValue / Scale;
    public static implicit operator float(Fixed64 value) => (float)value.InternalValue / Scale;
    public static implicit operator double(Fixed64 value) => (double)value.InternalValue / Scale;
    public static implicit operator decimal(Fixed64 value) => (decimal)value.InternalValue / Scale;
    #endregion 显式/隐式转化
    
    #region Format
    
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => AsDecimal().TryFormat(destination, out charsWritten, format, provider);
    
    public static Fixed64 Parse(string s, IFormatProvider? provider)
        => new Fixed64(decimal.Parse(s, NumberStyles.Float, provider));
    
    public static Fixed64 Parse(string s, NumberStyles style, IFormatProvider? provider)
        => new Fixed64(decimal.Parse(s, style, provider));
    
    public static Fixed64 Parse(ReadOnlySpan<char> s, IFormatProvider? provider) 
        => new Fixed64(decimal.Parse(s, NumberStyles.Float, provider));
    
    public static Fixed64 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => new Fixed64(decimal.Parse(s, style, provider));
    
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Fixed64 result)
        => TryParse(s, NumberStyles.Float, provider, out result);
    
    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Fixed64 result)
    {
        bool success = decimal.TryParse(s, style, provider, out decimal value);
        result = new Fixed64(value);
        return success;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Fixed64 result)
        => TryParse(s, NumberStyles.Float, provider, out result);
    
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Fixed64 result)
    {
        bool success = decimal.TryParse(s, style, provider, out decimal value);
        result = new Fixed64(value);
        return success;
    }
    
    #endregion Format
    
    #region Object
    
    public override string ToString() => AsDecimal().ToString(CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) => AsDecimal().ToString(format, formatProvider);
    
    static bool INumberBase<Fixed64>.TryConvertFromChecked<TOther>(TOther value, out Fixed64 result)
    {
        bool success = TryConvert(value, out decimal temp);
        result = new Fixed64(temp);
        return success;

        static bool TryConvert<TSelf>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TSelf : INumberBase<TSelf>
            => TSelf.TryConvertFromChecked(value, out result);
    }

    public static bool TryConvertFromSaturating<TOther>(TOther value, out Fixed64 result) where TOther : INumberBase<TOther>
    {
        bool success = TryConvert(value, out decimal temp);
        result = new Fixed64(temp);
        return success;

        static bool TryConvert<TSelf>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TSelf : INumberBase<TSelf>
            => TSelf.TryConvertFromSaturating(value, out result);
    }

    public static bool TryConvertFromTruncating<TOther>(TOther value, out Fixed64 result) where TOther : INumberBase<TOther>
    {
        bool success = TryConvert(value, out decimal temp);
        result = new Fixed64(temp);
        return success;

        static bool TryConvert<TSelf>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TSelf : INumberBase<TSelf>
            => TSelf.TryConvertFromTruncating(value, out result);
    }

    public static bool TryConvertToChecked<TOther>(Fixed64 value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
    {
        return TryConvert((decimal)value, out result);
        static bool TryConvert<TSelf>(TSelf value, [MaybeNullWhen(false)] out TOther result) where TSelf : INumberBase<TSelf>
            => TSelf.TryConvertToChecked(value, out result);
    }

    public static bool TryConvertToSaturating<TOther>(Fixed64 value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
    {
        return TryConvert((decimal)value, out result);
        static bool TryConvert<TSelf>(TSelf value, [MaybeNullWhen(false)] out TOther result) where TSelf : INumberBase<TSelf>
            => TSelf.TryConvertToSaturating(value, out result);
    }

    public static bool TryConvertToTruncating<TOther>(Fixed64 value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
    {
        return TryConvert((decimal)value, out result);
        static bool TryConvert<TSelf>(TSelf value, [MaybeNullWhen(false)] out TOther result) where TSelf : INumberBase<TSelf>
            => TSelf.TryConvertToTruncating(value, out result);
    }
    
    #endregion Object

    #region NumberInterface
    
    public static bool IsCanonical(Fixed64 value) => true;

    public static bool IsComplexNumber(Fixed64 value) => false;

    public static bool IsEvenInteger(Fixed64 value)
    {
        long internalValue = value.InternalValue;
        long integer = internalValue / Scale;
        if (integer * Scale != internalValue) return false;
        return (integer & 1) == 0;
    }

    public static bool IsFinite(Fixed64 value) => true;

    public static bool IsImaginaryNumber(Fixed64 value) => false;

    public static bool IsInfinity(Fixed64 value) => false;

    public static bool IsInteger(Fixed64 value) 
        => value.InternalValue / Scale * Scale == value.InternalValue;

    public static bool IsNaN(Fixed64 value) => false;

    public static bool IsNegative(Fixed64 value) => value.InternalValue < 0;

    public static bool IsNegativeInfinity(Fixed64 value) => false;

    public static bool IsNormal(Fixed64 value) => value.InternalValue != 0;

    public static bool IsOddInteger(Fixed64 value)
    {
        long internalValue = value.InternalValue;
        long integer = internalValue / Scale;
        if (integer * Scale != internalValue) return false;
        return (integer & 1) != 0;
    }

    public static bool IsPositive(Fixed64 value) => value.InternalValue > 0;

    public static bool IsPositiveInfinity(Fixed64 value) => false;

    public static bool IsRealNumber(Fixed64 value) => true;

    public static bool IsSubnormal(Fixed64 value) => false;

    public static bool IsZero(Fixed64 value) => value.InternalValue is 0;

    public static Fixed64 MaxMagnitude(Fixed64 x, Fixed64 y)
    {
        Fixed64 ax = Abs(x);
        Fixed64 ay = Abs(y);
        if (ax > ay) return x;
        if (ax == ay) return IsNegative(x) ? y : x;
        return y;
    }

    static Fixed64 INumberBase<Fixed64>.MaxMagnitudeNumber(Fixed64 x, Fixed64 y) => MaxMagnitude(x, y);

    public static Fixed64 MinMagnitude(Fixed64 x, Fixed64 y)
    {
        Fixed64 ax = Abs(x);
        Fixed64 ay = Abs(y);
        if (ax < ay) return x;
        if (ax == ay) return IsNegative(x) ? x : y;
        return y;
    }

    static Fixed64 INumberBase<Fixed64>.MinMagnitudeNumber(Fixed64 x, Fixed64 y) => MinMagnitude(x, y);

    #endregion NumberInterface
    
}