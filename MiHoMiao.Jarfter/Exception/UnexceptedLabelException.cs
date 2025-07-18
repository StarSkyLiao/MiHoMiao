namespace MiHoMiao.Jarfter.Exception;

public class UnexceptedLabelException(string labelName) : System.Exception
{
    public override string Message => $"Label '{labelName}' can not found or create in stack.";
}