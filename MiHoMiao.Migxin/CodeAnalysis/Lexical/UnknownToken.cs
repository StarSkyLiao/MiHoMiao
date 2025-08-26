namespace MiHoMiao.Migxin.CodeAnalysis.Lexical;

internal class UnknownToken((int Line, int Column) position, string text) : MigxinDiagnostic(position)
{
    public override string DiagnosticMessage => $"UnknownToken:>>{text}<<";
}