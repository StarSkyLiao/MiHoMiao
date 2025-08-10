namespace MiHoMiao.Migxn.Exceptions.Grammar;

public class TypeNotFitException(int position, Type type1, Type type2) : Exception
{
    public override string Message => $"Position {position}: type {type1.Name} and  {type2.Name} are not fit.";
}