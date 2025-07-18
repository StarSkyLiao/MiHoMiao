namespace MiHoMiao.Jarfter.Exception;

public class EmptyCalculationStackException : System.Exception
{
    public override string Message => $"CalculationStack is empty, but you are popping it.";
}