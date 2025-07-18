namespace MiHoMiao.Jarfter.Exception;

public class InvalidTypeException<T>(string typeString) : System.Exception
{
    public override string Message => $"Variable {{>>{typeString}<<}} can not be cast to {{>>{typeof(T).FullName}<<}}";
}