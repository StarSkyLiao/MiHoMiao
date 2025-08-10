namespace MiHoMiao.Migxn.Exceptions.Grammar;

public class NotStatementException(int position) : Exception
{
    public override string Message => $"Position {position}: only assignment, call, new() are recognized as statements.";
}