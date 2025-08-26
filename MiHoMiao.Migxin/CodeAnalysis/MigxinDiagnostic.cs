namespace MiHoMiao.Migxin.CodeAnalysis;

internal abstract class MigxinDiagnostic((int Line, int Column) position) : Exception
{
    public sealed override string Message => $"{position}: {DiagnosticMessage}";
    
    public abstract string DiagnosticMessage { get; }

    public static implicit operator DiagnosticBag(MigxinDiagnostic exception) => new DiagnosticBag(exception);

}