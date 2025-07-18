namespace MiHoMiao.Jarfter.Exception;

public class VariableNotDeclaredException(string name) : System.Exception
{
    public override string Message => $"Variable {{>>{name}<<}} is not declared.";
}