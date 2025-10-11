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
            new Fixed64(0.5).Tan();
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
    /// 计算并返回指定 <see cref="Fixed64"/> 定点数的算术平方根。
    /// </summary>
    /// <remarks>
    /// 实现采用牛顿迭代法进行逼近，初始猜测值通过 <see cref="Math.Sqrt(double)"/> 获得，
    /// 迭代终止条件为相邻两次迭代结果相等或出现循环。
    /// </remarks>
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
    
    /// <summary>
    /// 将指定 <see cref="Fixed64"/> 值按另一个 <see cref="Fixed64"/> 指数进行幂运算。
    /// </summary>
    /// <remarks>
    /// 算法将指数拆分为整数部分与小数部分：
    /// <list type="bullet">
    /// <item>整数部分使用 "快速幂" 计算；</item>
    /// <item>小数部分通过对数-指数公式 <c>Exp(frac * Ln(value))</c> 计算；</item>
    /// <item>最后将两部分结果相乘并根据指数符号决定是否取倒数。</item>
    /// </list>
    /// </remarks>
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
    
    /// <summary>
    /// 返回 e（自然对数的底）的指定 <see cref="Fixed64"/> 次幂。
    /// </summary>
    /// <remarks>
    /// 内部实现先计算绝对值的指数，再根据原始符号决定是否取倒数，以保证单调性与精度。
    /// </remarks>
    public static Fixed64 Exp(this Fixed64 value)
    {
        if (value.IsZero()) return Fixed64.One;
        if (value.IsOne()) return Fixed64.E;
        Fixed64 result = value.Abs().ExpInternal();
        if (value.IsNegative()) result = Fixed64.One / result;
        return result;
    }
    
    /// <summary>
    /// 计算并返回指定 <see cref="Fixed64"/> 值以 e 为底的自然对数。
    /// </summary>
    public static Fixed64 Ln(this Fixed64 value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        if (value.IsOne()) return Fixed64.Zero;
        return value >= Fixed64.One ? LnWhenGreaterThanOne(value) : LnWhenLessThanOne(value);
    }

    /// <summary>
    /// 计算并返回指定 <see cref="Fixed64"/> 值以 2 为底的对数。
    /// </summary>
    /// <remarks>
    /// 通过换底公式实现：<c>Log2(value) = Ln(value) * Ln2Rate</c>，
    /// 其中 <see cref="Fixed64.Ln2Rate"/> 为 1/ln(2) 的定点常数。
    /// </remarks>
    public static Fixed64 Log2(this Fixed64 value) => value.Ln() * Fixed64.Ln2Rate;
    
    public static Fixed64 Sin(this Fixed64 value)
    {
        value = value.CircleClamp(0, 2 * Fixed64.Pi);

        switch (value.InternalValue * 2)
        {
            case 0 or 2 * Fixed64.PiValue or 4 * Fixed64.PiValue: return Fixed64.Zero;
            case Fixed64.PiValue: return Fixed64.One;
            case 3 * Fixed64.PiValue: return Fixed64.NegativeOne;
        }

        Fixed64 result = Fixed64.Zero;
        long doubleIteration = 0;
        Fixed64 xSquared = value * value;
        Fixed64 nextAdd = Fixed64.Zero;

        while (true)
        {
            if (doubleIteration == 0) nextAdd = value;
            else nextAdd *= -xSquared / (doubleIteration * doubleIteration + doubleIteration);
            if (nextAdd.IsZero()) break;
            result += nextAdd;
            doubleIteration += 2;
        }

        return result;
    }
    
    public static Fixed64 Cos(this Fixed64 value)
    {
        value = value.CircleClamp(0, 2 * Fixed64.Pi);

        switch (value.InternalValue * 2)
        {
            case 0 or 4 * Fixed64.PiValue: return Fixed64.One;
            case Fixed64.PiValue or 3 * Fixed64.PiValue: return Fixed64.Zero;
            case 2 * Fixed64.PiValue: return Fixed64.NegativeOne;
        }

        Fixed64 result = Fixed64.Zero;
        int doubleIteration = 0;
        Fixed64 xSquared = value * value;
        Fixed64 nextAdd = Fixed64.Zero;

        while (true)
        {
            if (doubleIteration == 0) nextAdd = Fixed64.One;
            else nextAdd *= -xSquared / (doubleIteration * doubleIteration - doubleIteration);
            if (nextAdd.IsZero()) break;
            result += nextAdd;
            doubleIteration += 2;
        }

        return result;
    }
    
    public static Fixed64 Tan(this Fixed64 value)
    {
        value = value.CircleClamp(0, 2 * Fixed64.Pi);
        Fixed64 sin = value.Sin();
        Fixed64 cos = value.Cos();
        return sin / cos;
    }
    
    private static Fixed64 ExpInternal(this Fixed64 value)
    {
        Debug.Assert(value.IsPositive());
        Fixed64 integer = value.Truncate();
        // 大于 1 的小数
        if (value.InternalValue > Fixed64.Scale && integer != value) return Exp(integer) * Exp(value - integer);
        // 整数
        if (value == integer) return ExpBySquaring(Fixed64.E, value);
        // 0 ~ 1 的小数
        int iteration = 0;
        Fixed64 result = Fixed64.Zero;
        Fixed64 nextAdd = Fixed64.One;
        while (true)
        {
            if (iteration > 0) nextAdd *= value / iteration;
            if (nextAdd.IsZero()) break;
            result += nextAdd;
            ++iteration;
        }
        return result;
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