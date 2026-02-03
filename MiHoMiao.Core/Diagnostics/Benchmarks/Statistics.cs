namespace MiHoMiao.Core.Diagnostics.Benchmarks;

internal static class Statistics
{
    internal static string Fmt(this decimal value, string? format = null) => value switch
    {
        >= 1e+15m => $"{(value / 1e+15m).ToString(format)}T",
        >= 1e+12m and < 1e+15m => $"{(value / 1e+12m).ToString(format)}T",
        >= 1e+9m and < 1e+12m => $"{(value / 1e+9m).ToString(format)}G",
        >= 1e+6m and < 1e+9m => $"{(value / 1e+6m).ToString(format)}M",
        >= 1e+3m and < 1e+6m => $"{(value / 1e+3m).ToString(format)}K",
        >= 1e+0m and < 1e+3m => $"{(value / 1).ToString(format)}",
        >= 1e-3m and < 1e+0m => $"{(value / 1e-3m).ToString(format)}m",
        >= 1e-6m and < 1e-3m => $"{(value / 1e-6m).ToString(format)}μ",
        >= 1e-9m and < 1e-6m => $"{(value / 1e-9m).ToString(format)}n",
        < 1e-9m => $"{(value / 1e-12m).ToString(format)}n",
    };
    
    /// <summary>
    /// 计算总体方差 (Population Variance) - 除以 N
    /// 适用于完整数据集
    /// </summary>
    internal static decimal VariancePopulation(decimal[] data)
    {
        decimal mean = 0;
        decimal m2 = 0;
        
        for (int i = 0; i < data.Length; i++)
        {
            decimal x = data[i];
            decimal delta = x - mean;
            mean += delta / (i + 1);
            decimal delta2 = x - mean;
            m2 += delta * delta2;
        }

        return (decimal)Math.Sqrt((double)m2 / data.Length);
    }
}