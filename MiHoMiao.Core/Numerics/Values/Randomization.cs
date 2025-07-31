//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------

namespace MiHoMiao.Core.Numerics.Values;

/// <summary>
/// 对 System.Random 类的一个简单封装，更方便地使用随机数。
/// </summary>
public static class Randomization
{
    private static Random s_Random = new Random();

    /// <summary>
    /// 设置当前的种子值
    /// </summary>
    public static void SetSeed(int seed)=> s_Random = new Random(seed);
    
    /// <summary>
    /// 返回 0.0f 到 1.0f 之间的某一个单精度浮点数
    /// </summary>
    public static float Single(this Random random) => random.NextSingle();

    /// <summary>
    /// 返回 0.0f 到 1.0f 之间的某一个单精度浮点数
    /// </summary>
    public static float Single() => (float)s_Random.NextDouble();
    
    /// <summary>
    /// 返回 0.0 到 1.0 之间的某一个双精度浮点数
    /// </summary>
    public static double Double(this Random random) => random.NextDouble();

    /// <summary>
    /// 返回 0.0 到 1.0 之间的某一个双精度浮点数
    /// </summary>
    public static double Double() => s_Random.NextDouble();

    /// <summary>
    /// 返回 minInclusive 到 maxInclusive 之间的某一个单精度浮点数
    /// </summary>
    public static float Range(float minInclusive, float maxInclusive)
        => Range(s_Random, minInclusive, maxInclusive);

    /// <summary>
    /// 返回 minInclusive 到 maxInclusive 之间的某一个单精度浮点数
    /// </summary>
    public static float Range(this Random random, float minInclusive, float maxInclusive)
        => random.NextSingle() * (maxInclusive - minInclusive) + minInclusive;
    
    /// <summary>
    ///     返回 minInclusive 到 maxInclusive 之间的某一个双精度浮点数
    /// </summary>
    public static double Range(double minInclusive, double maxInclusive)
        => Range(s_Random, minInclusive, maxInclusive);
    
    /// <summary>
    /// 返回 minInclusive 到 maxInclusive 之间的某一个双精度浮点数
    /// </summary>
    public static double Range(this Random random, double minInclusive, double maxInclusive)
        => random.NextDouble() * (maxInclusive - minInclusive) + minInclusive;

    /// <summary>
    /// 返回 minInclusive 到 maxInclusive 之间的某一个整数
    /// </summary>
    public static int Range(int minInclusive, int maxExclusive)
        => s_Random.Next(minInclusive, maxExclusive);

    /// <summary>
    /// 返回 minInclusive 到 maxInclusive 之间的某一个整数
    /// </summary>
    public static int Range(this Random random, int minInclusive, int maxExclusive)
        => random.Next(minInclusive, maxExclusive);

    /// <summary>
    /// 返回 num 随机偏差实际值 delta 后的结果
    /// </summary>
    public static float Delta(this float num, float delta)
        => Range(s_Random, num - delta, num + delta);
    
    /// <summary>
    /// 返回 num 随机偏差实际值 delta 后的结果
    /// </summary>
    public static double Delta(this double num, double delta)
        => Range(s_Random, num - delta, num + delta);

    /// <summary>
    /// 返回 num 随机偏差实际值 delta 后的结果
    /// </summary>
    public static float Delta(this Random random, float num, float delta)
        => random.Range(num - delta, num + delta);
    
    /// <summary>
    /// 返回 num 随机偏差实际值 delta 后的结果
    /// </summary>
    public static double Delta(this Random random, double num, double delta)
        => random.Range(num - delta, num + delta);

    /// <summary>
    /// 返回 num 随机偏差百分比 delta 后的结果
    /// </summary>
    public static float DeltaPct(float num, float delta)
        => Range(s_Random, num * (1 - delta), num * (1 + delta));

    /// <summary>
    /// 返回 num 随机偏差百分比 delta 后的结果
    /// </summary>
    public static float DeltaPct(this Random random, float num, float delta)
        => random.Range(num * (1 - delta), num * (1 + delta));

    /// <summary>
    /// 有 chance 的概率返回 True ;
    /// 否则, 返回 false
    /// </summary>
    public static bool Try(float chance) => chance > s_Random.Single();
    
    /// <summary>
    /// 有 chance 的概率返回 True ;
    /// 否则, 返回 false
    /// </summary>
    public static bool Try(double chance) => chance > s_Random.Double();

    /// <summary>
    /// 有 chance 的概率返回 True ;
    /// 否则, 返回 false
    /// </summary>
    public static bool Try(this Random random, float chance) => chance > random.Range(0f, 1f);
    
    /// <summary>
    /// 返回枚举中的一个随机元素
    /// </summary>
    public static T? Select<T>(IEnumerable<T> enumerable)
    {
        T[] array = enumerable as T[] ?? enumerable.ToArray();
        return array.Length == 0 ? default : array[s_Random.Next(0, array.Length)];
    }
    
    /// <summary>
    /// 返回枚举中的一个随机元素
    /// </summary>
    public static T? Select<T>(this Random random, IEnumerable<T> enumerable)
    {
        T[] array = enumerable as T[] ?? enumerable.ToArray();
        return array.Length == 0 ? default : array[random.Range(0, array.Length)];
    }
    
}