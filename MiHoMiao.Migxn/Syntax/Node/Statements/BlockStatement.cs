using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Node.Statements;

public record BlockStatement(List<StatementNode> Statements, ReadOnlyMemory<char> Text) 
    : StatementNode(Statements.ConvertAll(MigxnNode (item) => item), Text)
{
    internal override void EmitCode(MigxnContext context, ILGenerator generator)
    {
        context.Variables.PushScope();
        foreach (var statementNode in Statements) statementNode.EmitCode(context, generator);
        context.Variables.PopScope();
    }
}