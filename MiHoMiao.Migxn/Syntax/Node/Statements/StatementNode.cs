using System.Diagnostics;
using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Node.Statements;

public record StatementNode(List<MigxnNode> Nodes, ReadOnlyMemory<char> Text) : MigxnNode(Text)
{
    protected override List<MigxnNode> Children() => Nodes;
    
    internal override void EmitCode(MigxnContext context, ILGenerator generator)
    {
        IKeyword? keyword = Nodes[0] as IKeyword;
        Debug.Assert(keyword != null);
        keyword.EmitCode(Nodes, context, generator);
    }
    
}