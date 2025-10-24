using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.Analysis.Diagnostic;

public sealed class DiagnosticBag : Exception
{
    internal DiagnosticBag(AnalysisException baseException) => DiagnosticsContained = [baseException];

    internal readonly List<AnalysisException> DiagnosticsContained;

    internal void Attach(AnalysisException diagnostic) => DiagnosticsContained.Add(diagnostic);

    public override string Message => DiagnosticsContained.GenericViewer(
        item => item.Message, "", "", "\nat:   "
    );
    
}