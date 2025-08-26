namespace MiHoMiao.Migxin.CodeAnalysis.Grammar;

internal class ShouldBe((int Line, int Column) position, string text) : MigxinDiagnostic(position)
{
    public override string DiagnosticMessage => $"Here should be: {text}";
}