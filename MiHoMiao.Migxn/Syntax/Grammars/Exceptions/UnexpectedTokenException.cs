namespace MiHoMiao.Migxn.Syntax.Grammars.Exceptions;

public class UnexpectedTokenException<T>(string token) : Exception
{

    public override string Message => $"Unexpected token:{token}. Excepted for {typeof(T).Name}";
    
}