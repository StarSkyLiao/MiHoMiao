namespace MiHoMiao.Migxn.CodeAnalysis.Parser;

public class UndefinedType((int Line, int Column) position, string text) : Exception
{
    public override string Message => $"UnclosedString at line {position.Line} column {position.Column}. (:{text}).";
}