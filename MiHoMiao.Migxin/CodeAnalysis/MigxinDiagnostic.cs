namespace MiHoMiao.Migxin.CodeAnalysis;

internal abstract class MigxinDiagnostic : Exception
{
    public abstract override string Message { get; }
}