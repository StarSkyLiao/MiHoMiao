namespace MiHoMiao.Core.Diagnostics;

[Flags]
public enum PerfTestOption: byte
{
    Warm         = 0b0000_0001,
    Sequence     = 0b0000_0010,
    Best75       = 0b0000_0100,
}