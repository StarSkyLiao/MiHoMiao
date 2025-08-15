using MiHoMiao.Core.Collections.Tool;

namespace MiHoMiao.Migxn.Syntax.Grammars;

public record BadTree(List<MigxnNode> ChildNodes)
    : MigxnTree(ChildNodes.GenericViewer(node => node.Text.ToString(), "", "", "").AsMemory(),
        ChildNodes[0].Index, ChildNodes[0].Position)
{
    internal override IEnumerable<MigxnNode> Children() => ChildNodes;

    protected override string SelfString() =>
        $"{GetType().Name} >>>> (Line: {Position.Line}, Column: {Position.Column}): {Text}>>><<<";

}