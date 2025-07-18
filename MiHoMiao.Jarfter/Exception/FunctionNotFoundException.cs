namespace MiHoMiao.Jarfter.Exception;

public class FunctionNotFoundException(string function) : System.Exception
{
    
    public override string Message => $"Function {function} is not found.";
    
}