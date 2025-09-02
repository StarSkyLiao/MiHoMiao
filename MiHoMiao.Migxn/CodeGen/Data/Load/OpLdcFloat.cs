using System.Globalization;

namespace MiHoMiao.Migxn.CodeGen.Data.Load;

internal class OpLdcFloat(double value) : OpLdc
{
    public readonly double Value = value;
    public override string ToString() => $"{"ldc.float",-12}{Value}";
    public override string AsString() => Value.ToString(CultureInfo.CurrentCulture);
}