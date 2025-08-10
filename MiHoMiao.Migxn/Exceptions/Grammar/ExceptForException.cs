namespace MiHoMiao.Migxn.Exceptions.Grammar;

public class ExceptForException(int position, string excepted) : Exception
{
    public override string Message => $"Position {position}: here should be {excepted}!";
}