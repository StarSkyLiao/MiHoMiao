namespace MiHoMiao.Migxn.Exceptions.Lexer;

public class UnclosedStringException(int position) : Exception
{
    public override string Message => $"Position {position}: string is not closed.";
}