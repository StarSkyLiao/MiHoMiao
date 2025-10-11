using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.xUnit.Core.Numerics.Values;

/**
| Method      | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD |
|------------ |-----------:|----------:|----------:|-----------:|------:|--------:|
| Add         |  0.0002 ns | 0.0005 ns | 0.0004 ns |  0.0000 ns |     ? |       ? |
| Sub         |  0.0050 ns | 0.0050 ns | 0.0044 ns |  0.0040 ns |     ? |       ? |
| Mul         |  0.5770 ns | 0.0110 ns | 0.0103 ns |  0.5741 ns |     ? |       ? |
| Div         |  2.4111 ns | 0.0037 ns | 0.0035 ns |  2.4107 ns |     ? |       ? |
| Double_Add  |  0.0013 ns | 0.0021 ns | 0.0020 ns |  0.0000 ns |     ? |       ? |
| Double_Sub  |  0.0001 ns | 0.0002 ns | 0.0002 ns |  0.0000 ns |     ? |       ? |
| Double_Mul  |  0.0001 ns | 0.0003 ns | 0.0002 ns |  0.0000 ns |     ? |       ? |
| Double_Div  |  0.0003 ns | 0.0007 ns | 0.0006 ns |  0.0000 ns |     ? |       ? |
| Decimal_Add |  3.5514 ns | 0.0055 ns | 0.0049 ns |  3.5514 ns |     ? |       ? |
| Decimal_Sub |  3.7365 ns | 0.0026 ns | 0.0022 ns |  3.7372 ns |     ? |       ? |
| Decimal_Mul |  7.0035 ns | 0.0309 ns | 0.0289 ns |  6.9942 ns |     ? |       ? |
| Decimal_Div | 48.0771 ns | 0.1566 ns | 0.1388 ns | 48.0297 ns |     ? |       ? |
 */

// [SimpleJob(warmupCount: 3, iterationCount: 5, invocationCount: 1048576)]
[SimpleJob(RuntimeMoniker.Net90)] 
public class Fixed64OpsBench
{
    public static void Run() => BenchmarkRunner.Run<Fixed64OpsBench>();
    
    // 测试数据：故意弄成非 0、非整数、带小数
    private readonly Fixed64 m_Fixed64A = 4121123.45678912;
    private readonly Fixed64 m_Fixed64B = 23178.90123456;

    /* ---------- 四则运算 ---------- */
    [Benchmark(Baseline = true)]
    public Fixed64 Add() => m_Fixed64A + m_Fixed64B;

    [Benchmark]
    public Fixed64 Sub() => m_Fixed64A - m_Fixed64B;

    [Benchmark]
    public Fixed64 Mul() => m_Fixed64A * m_Fixed64B;

    [Benchmark]
    public Fixed64 Div() => m_Fixed64A / m_Fixed64B;

    /* ---------- 与原生 double 对比 ---------- */
    private readonly double m_DoubleA = 123.45678912;
    private readonly double m_DoubleB = 78.90123456;

    [Benchmark]
    public double Double_Add() => m_DoubleA + m_DoubleB;
    
    [Benchmark]
    public double Double_Sub() => m_DoubleA - m_DoubleB;

    [Benchmark]
    public double Double_Mul() => m_DoubleA * m_DoubleB;
    
    [Benchmark]
    public double Double_Div() => m_DoubleA / m_DoubleB;
    
    /* ---------- 与原生 decimal 对比 ---------- */
    private readonly decimal m_DecimalA = 123.45678912m;
    private readonly decimal m_DecimalB = 78.90123456m;

    [Benchmark]
    public decimal Decimal_Add() => m_DecimalA + m_DecimalB;
    
    [Benchmark]
    public decimal Decimal_Sub() => m_DecimalA - m_DecimalB;

    [Benchmark]
    public decimal Decimal_Mul() => m_DecimalA * m_DecimalB;
    
    [Benchmark]
    public decimal Decimal_Div() => m_DecimalA / m_DecimalB;
}