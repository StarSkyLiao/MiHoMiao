using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.Exceptions.Grammar;

public class ExceptForException(BadTree badTree, string excepted) : Exception
{
    public override string Message => $"Position {badTree.Index}: here should be {excepted}!";
}