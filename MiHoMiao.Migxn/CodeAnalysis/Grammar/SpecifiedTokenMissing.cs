using MiHoMiao.Migxn.Syntax;
using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.CodeAnalysis.Grammar;

public class SpecifiedTokenMissing(List<MigxnNode> badTree, string target) : BadMigxnTree
{
    public override List<MigxnNode> MigxnTree => badTree;
    
    public override string Message => $"Specified token Missing : at line {badTree[^1].Position.Line} column {badTree[^1].NextColumn}. " +
                                      $"Here should be {target}!\n\r{new BadTree(MigxnTree)}";

    public static Diagnostic<T> Create<T>(List<MigxnNode> childNodes, string target) where T : class
        => new Diagnostic<T>(new SpecifiedTokenMissing(childNodes, target));

}