using MiHoMiao.Migxn.Syntax;
using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.CodeAnalysis.Grammar;

public class BadStatement(MigxnTree tree) : BadMigxnTree
{

    public override string Message => $"Unexpected statement:{tree.Text}. Only assignment, call, declaration are regarded as statements.";

    public override List<MigxnNode> MigxnTree { get; } = [tree];

}