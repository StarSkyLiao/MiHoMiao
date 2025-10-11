using System.Diagnostics;
using System.Numerics;
using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Core.Numerics.Values;

public static class Fixed64Math
{
    public static void Perf()
    {
        TimeTest.RunTest(() =>
        {
            new Fixed64(0.5).Log2();
        }, "", 100, TimeTest.RunTestOption.Warm);
    }
    
    /// <summary>
    /// 返回指定定点数的一半.
    /// </summary>
    public static Fixed64 Half(this Fixed64 value) => new Fixed64(value.InternalValue >> 1, true);
    
    /// <summary>
    /// 返回指定定点数的双倍.
    /// </summary>
    public static Fixed64 Double(this Fixed64 value) => new Fixed64(value.InternalValue << 1, true);
    
    /// <summary>
    /// 返回指定定点数的绝对值.
    /// </summary>
    public static Fixed64 Abs(this Fixed64 value) => new Fixed64(Math.Abs(value.InternalValue), true);
    
    /// <summary>
    /// 返回指定定点数的整数部分.
    /// </summary>
    public static Fixed64 Truncate(this Fixed64 value) => new Fixed64(value.InternalValue / Fixed64.Scale);

    /// <summary>
    /// 返回指定定点数是否为正数.
    /// </summary>
    public static bool IsPositive(this Fixed64 value) => value.InternalValue > 0;
    
    /// <summary>
    /// 返回指定定点数是否为负数.
    /// </summary>
    public static bool IsNegative(this Fixed64 value) => value.InternalValue < 0;
    
    /// <summary>
    /// 返回指定定点数是否为 0.
    /// </summary>
    public static bool IsZero(this Fixed64 value) => value.InternalValue is 0;
    
    /// <summary>
    /// 返回指定定点数是否为非 0.
    /// </summary>
    public static bool NotZero(this Fixed64 value) => value.InternalValue is not 0;

    /// <summary>
    /// 返回指定定点数是否为 1.
    /// </summary>
    public static bool IsOne(this Fixed64 value) => value.InternalValue is Fixed64.Scale;
    
    /// <summary>
    /// 返回指定定点数的算术平方根.
    /// 如果定点数的值小于 0, 则抛出异常
    /// </summary>
    public static Fixed64 Sqrt(this Fixed64 value)
    {
        long internalValue = value.InternalValue;
        ArgumentOutOfRangeException.ThrowIfNegative(internalValue);
        if (internalValue == 0) return Fixed64.Zero;

        Fixed64 halfS = value.Half();
        Fixed64 lastX = Fixed64.NegativeOne;
        Fixed64 nextX;
        
        Fixed64 rounded = Math.Sqrt(value.AsFloat64());

        while (true)
        {
            nextX = rounded.Half() + halfS / rounded;
            if (nextX == rounded || nextX == lastX) break;
            lastX = rounded;
            rounded = nextX;
        }

        return nextX;
    }
    
    public static Fixed64 Pow(this Fixed64 value, Fixed64 exponent)
    {
        if (exponent.IsZero()) return Fixed64.One;
        if (exponent.IsOne()) return value;

        bool neg = exponent.IsNegative();
        exponent = exponent.Abs();

        Fixed64 whole = exponent.Truncate();
        Fixed64 frac = exponent - whole;

        Fixed64 intPart  = whole.IsZero() ? Fixed64.One : ExpBySquaring(value, whole);
        Fixed64 fracPart = frac.IsZero() ? Fixed64.One : Exp(frac * Ln(value));

        Fixed64 r = intPart * fracPart;
        return neg ? Fixed64.One / r : r;
    }

    private static Fixed64 ExpBySquaring(Fixed64 self, Fixed64 other)
    {
        Debug.Assert(other.InternalValue > 0);
        Fixed64 result = Fixed64.One;
        Fixed64 multiplier = self;

        while (other.IsPositive())
        {
            if (Fixed64.IsOddInteger(other))
            {
                result *= multiplier;
                --other;
                if (other.InternalValue == 0) break;
            }
            multiplier *= multiplier;
            other = other.Half();
        }

        return result;
    }
    
    public static Fixed64 Exp(this Fixed64 value)
    {
        if (value.IsZero()) return Fixed64.One;
        if (value.IsOne()) return Fixed64.E;
        
        Fixed64 result;

        bool reciprocal = value.IsNegative();
        value = value.Abs();
        
        Fixed64 integer = new Fixed64(value.AsInt64());
        
        if (value.InternalValue > Fixed64.Scale && integer != value)
        {
            result = Exp(integer) * Exp(value - integer);
        }
        else if (value == integer)
        {
            result = ExpBySquaring(Fixed64.E, value);
        }
        else
        {
            int iteration = 0;
            Fixed64 nextAdd = result = Fixed64.Zero;

            while (true)
            {
                if (iteration == 0) nextAdd = Fixed64.One;
                else nextAdd *= value / iteration;
                if (nextAdd == 0) break;
                result += nextAdd;
                iteration += 1;
            }
        }
        
        if (reciprocal) result = 1 / result;

        return result;
    }
    
    /// <summary>
    /// 返回 value 以 e 为底的对数，定点数格式（8 位小数）。
    /// value ≤ 0 时抛 ArgumentOutOfRangeException。
    /// </summary>
    public static Fixed64 Ln(this Fixed64 value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        if (value.IsOne()) return Fixed64.Zero;
        return value >= Fixed64.One ? LnWhenGreaterThanOne(value) : LnWhenLessThanOne(value);
    }

    /// <summary>
    /// 返回 value 以 2 为底的对数，定点数格式（8 位小数）。
    /// value ≤ 0 时抛 ArgumentOutOfRangeException。
    /// </summary>
    public static Fixed64 Log2(this Fixed64 value) => value.Ln() * Fixed64.Ln2Rate;
    
    private static Fixed64 LnWhenGreaterThanOne(Fixed64 value)
    {
        Debug.Assert(value > Fixed64.One);
        Fixed64 power = Fixed64.Zero;
        Fixed64 x = value;
        while (x.InternalValue >= Fixed64.Scale)
        {
            x /= 10;
            ++power;
        }
        return LnWhenLessThanOne(x) + power * Fixed64.Ln10;
    }
    
    private static Fixed64 LnWhenLessThanOne(Fixed64 value)
    {
        Debug.Assert(value.IsPositive() && value < Fixed64.One);
        Fixed64 result = Fixed64.Zero;
        
        while (true)
        {
            Fixed64 last = result;
            Fixed64 exp = result.Exp();
            result += ((value - exp) / (value + exp)).Double();
            if (result == last) break;
        }

        return result;
    }
    
}