namespace MiHoMiao.Migxin.CodeAnalysis.Grammar;

internal class TypeUnclear((int Line, int Column) position, string varName) : MigxinDiagnostic(position)
{
    public override string DiagnosticMessage => $"Type of {varName} is not clearly.";
}