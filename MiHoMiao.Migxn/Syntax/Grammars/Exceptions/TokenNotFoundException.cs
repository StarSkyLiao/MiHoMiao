namespace MiHoMiao.Migxn.Syntax.Grammars.Exceptions;

public class TokenNotFoundException<T> : Exception
{
    public override string Message => $"Token type {typeof(T).Name} is not found forward!";

}