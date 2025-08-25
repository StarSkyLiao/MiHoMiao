using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxin.CodeAnalysis;

public sealed class DiagnosticBag : Exception
{
    internal DiagnosticBag(MigxinDiagnostic baseException) => DiagnosticsContained = [baseException];

    internal readonly List<MigxinDiagnostic> DiagnosticsContained;

    public override string Message => DiagnosticsContained.GenericViewer(
        item => item.Message, "", "", "\nat:   "
    );
    
}