namespace MiHoMiao.Core.Diagnostics;

public record struct PerfTestSetting(uint Iterations, PerfTestOption Option)
{
    public uint LoopCount { get; init; } = 1;
}