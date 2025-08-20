using MiHoMiao.Migxn.Syntax;
using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.CodeAnalysis.Grammar;

public class BadAssignment(MigxnTree tree) : BadMigxnTree
{

    public override string Message => $"Unexpected assignment:{tree.Text}. Only symbols are allowed to be placed in font of equal token.";

    public override List<MigxnNode> MigxnTree { get; } = [tree];

}