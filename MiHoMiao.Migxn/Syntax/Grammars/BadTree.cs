using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars;

internal record BadTree(List<MigxnNode> ChildNodes)
    : MigxnTree(ChildNodes.GenericViewer(node => node.Text.ToString(), "", "", "").AsMemory(),
        ChildNodes[0].Index, ChildNodes[0].Position)
{
    internal override IEnumerable<MigxnNode> Children() => ChildNodes;

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => throw new NotSupportedException();
    
    protected override string SelfString() =>
        $"{GetType().Name} >>>> (Line: {Position.Line}, Column: {Position.Column}): {Text}>>><<<";

}