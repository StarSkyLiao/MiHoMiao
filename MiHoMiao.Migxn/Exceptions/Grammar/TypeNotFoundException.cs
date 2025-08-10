namespace MiHoMiao.Migxn.Exceptions.Grammar;

public class TypeNotFoundException(int position) : Exception
{
    public override string Message => $"Position {position}: unrecognized type.";
}