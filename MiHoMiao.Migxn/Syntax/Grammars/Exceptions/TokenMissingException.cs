using MiHoMiao.Core.Diagnostics;

namespace MiHoMiao.Migxn.Syntax.Grammars.Exceptions;

public class TokenMissingException(BadTree badTree, string target) : Exception, IBadTreeException
{
    
    public MigxnTree MigxnTree => badTree;
    
    public override string Message => $"{MigxnTree}: here should be {target}!";

    public static ActionResult<T> Create<T>(List<MigxnNode> childNodes, string target) where T : class 
        => new TokenMissingException(new BadTree(childNodes), target);

}