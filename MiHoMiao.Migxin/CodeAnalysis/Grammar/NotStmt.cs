namespace MiHoMiao.Migxin.CodeAnalysis.Grammar;

internal class NotStmt((int Line, int Column) position) : MigxinDiagnostic(position)
{
    public override string DiagnosticMessage => "Only assignment, call, increment, decrement and new can be regarded as statements.";
}