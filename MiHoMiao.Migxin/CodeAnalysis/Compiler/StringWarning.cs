namespace MiHoMiao.Migxin.CodeAnalysis.Compiler;

internal class StringWarning((int Line, int Column) position, string text) : MigxinDiagnostic(position)
{
    public override string DiagnosticMessage => text;
}