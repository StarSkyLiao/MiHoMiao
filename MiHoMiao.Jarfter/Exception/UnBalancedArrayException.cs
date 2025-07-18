namespace MiHoMiao.Jarfter.Exception;

public class UnBalancedArrayException(string except, string actual) : System.Exception
{
    public override string Message => $"Array input is not balanced. Except for {{>{except}<}}, but find {{>{actual}<}}.";
    
    public static void ThrowAtEndOfLine(string except) => throw new UnBalancedArrayException(except, "End_Of_Line");
    
}