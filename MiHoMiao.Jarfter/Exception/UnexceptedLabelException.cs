namespace MiHoMiao.Jarfter.Exception;

public class UnexceptedLabelException(string labelName) : System.Exception
{
    public override string Message => $"Label '{labelName}' can not be found or created in stack.";
}