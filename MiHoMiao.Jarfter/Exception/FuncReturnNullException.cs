namespace MiHoMiao.Jarfter.Exception;

public class FuncReturnNullException(string funcName) : System.Exception
{
    public override string Message => $"Function {{{funcName}}} has no return value.";
}