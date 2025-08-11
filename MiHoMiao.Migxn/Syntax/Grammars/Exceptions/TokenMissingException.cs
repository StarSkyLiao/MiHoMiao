namespace MiHoMiao.Migxn.Syntax.Grammars.Exceptions;

public class TokenMissingException(BadTree badTree, string target) : Exception, IBadTreeException
{
    
    public MigxnTree MigxnTree => badTree;
    
    public override string Message => $"{MigxnTree}: here should be {target}!";

}