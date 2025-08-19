using MiHoMiao.Migxn.Syntax;
using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.CodeAnalysis.Grammar;

public class SpecifiedTokenMissing(string target, List<MigxnNode> badTree) : BadMigxnTree
{
    public override List<MigxnNode> MigxnTree => badTree;
    
    public override string Message => $"Specified token Missing : at line {MigxnTree[^1].Position.Line} column {MigxnTree[^1].NextColumn}. " +
                                      $"Here should be {target}!\n\r{new BadTree(MigxnTree)}";

    public static Diagnostic<T> Create<T>(string target, params Span<MigxnNode> children) where T : class
        => new Diagnostic<T>(new SpecifiedTokenMissing(target, [..children]));
    
    public static Diagnostic<T> Create<T>(string target, List<MigxnNode> children) where T : class
        => new Diagnostic<T>(new SpecifiedTokenMissing(target, children));

}