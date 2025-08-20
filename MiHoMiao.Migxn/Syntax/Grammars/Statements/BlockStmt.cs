using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Pair;

namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal record BlockStmt(CurlyOpenToken OpenToken, List<MigxnTree> ContainedStmts, CurlyCloseToken CloseToken)
    : MigxnStmt(ContainedStmts.GenericViewer(item => $"    {item.Text}", "\n{\n", "\n}", "\n").AsMemory(),
        OpenToken.Index, OpenToken.Position)
{
    internal override IEnumerable<MigxnNode> Children() => [OpenToken, ..ContainedStmts, CloseToken];

    public override IEnumerable<MigxnOpCode> AsOpCodes() => ContainedStmts.SelectMany(item => item.AsOpCodes());
}