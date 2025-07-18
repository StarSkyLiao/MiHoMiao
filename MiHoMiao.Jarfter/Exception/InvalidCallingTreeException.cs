namespace MiHoMiao.Jarfter.Exception;

public class InvalidCallingTreeException() : System.Exception
{
    public override string Message => $"You are trying to break the CallingTree!";
}