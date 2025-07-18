namespace MiHoMiao.Jarfter.Exception;

public class VariableMulDeclaredException(string name) : System.Exception
{
    public override string Message => $"Variable {{>>{name}<<}} is already declared.";
}