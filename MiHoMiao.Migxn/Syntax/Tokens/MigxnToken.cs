using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Node;

namespace MiHoMiao.Migxn.Syntax.Tokens;

public abstract record MigxnToken(int Position, ReadOnlyMemory<char> Text) : MigxnNode(Text)
{
    protected override IEnumerable<MigxnNode> Children() => [];

    internal override void EmitCode(MigxnContext context, ILGenerator generator) => throw new NotSupportedException();

}