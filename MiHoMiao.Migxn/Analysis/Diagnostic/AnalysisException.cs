namespace MiHoMiao.Migxn.Analysis.Diagnostic;

internal abstract class AnalysisException((int Line, int Column) position) : Exception
{
    public sealed override string Message => $"{position}: {DiagnosticMessage}";
    
    public abstract string DiagnosticMessage { get; }

    public static implicit operator DiagnosticBag(AnalysisException exception) => new DiagnosticBag(exception);

}