using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Node;
using MiHoMiao.Migxn.Syntax.Node.Statements;

namespace MiHoMiao.Migxn.Syntax.Tokens.Keywords;

public interface IKeyword
{
    public abstract StatementNode CollectNode(MigxnParser parser);

    public abstract void EmitCode(List<MigxnNode> nodes, MigxnContext context, ILGenerator generator);
}