using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.CodeAnalysis;

internal sealed class DiagnosticBag(Exception baseException) : Exception
{
    private readonly List<Exception> m_ExceptionsContained = [baseException];
    public override string Message => m_ExceptionsContained.GenericViewer(item => item.Message, "", "", "\n    ");
}