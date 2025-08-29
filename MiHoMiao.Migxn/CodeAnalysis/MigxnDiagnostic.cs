using Antlr4.Runtime;

namespace MiHoMiao.Migxn.CodeAnalysis;

internal class MigxnDiagnostic((int Line, int Column) position, string message) : Exception
{
    
    public override string ToString() => $"{GetType().Name,-25}: {Message}";

    public sealed override string Message => $"{position}->{message}";

    public static MigxnDiagnostic Create(IToken token, Exception exception) => new MigxnDiagnostic((token.Line, token.Column), exception.Message);

}