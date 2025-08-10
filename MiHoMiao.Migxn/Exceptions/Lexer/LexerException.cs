namespace MiHoMiao.Migxn.Exceptions.Lexer;

public class LexerException(int position, string info) : Exception
{
    public override string Message => $"Position {position}: {info}";
}