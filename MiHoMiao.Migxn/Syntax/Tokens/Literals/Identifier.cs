using System.Diagnostics;
using System.Reflection.Emit;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Exceptions.Grammar;
using MiHoMiao.Migxn.Reflection;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Node;
using MiHoMiao.Migxn.Syntax.Node.Expressions;
using MiHoMiao.Migxn.Syntax.Node.Statements;
using MiHoMiao.Migxn.Syntax.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

namespace MiHoMiao.Migxn.Syntax.Tokens.Literals;

public record Identifier(int Position, ReadOnlyMemory<char> Text) : LiteralToken(Position, Text), IKeyword
{
    StatementNode IKeyword.CollectNode(MigxnParser parser)
    {
        List<MigxnNode> nodes = new List<MigxnNode>(3) { this };
        MigxnNode? currentNode = parser.Current;
        if (currentNode is null) throw new NotStatementException(Position);
        switch (currentNode)
        {
            case EqlToken:
                nodes.Add(parser.Expect<EqlToken>());
                MigxnExpr? newValue = parser.ParseExpression(0, false);
                if (newValue is null) throw new ExceptForException(Position, nameof(MigxnExpr));
                nodes.Add(newValue);
                break;
        }
        return new StatementNode(nodes,
            nodes.GenericViewer(item => item.Text.ToString(),
                "", "", " ").AsMemory()
        );
    }

    void IKeyword.EmitCode(List<MigxnNode> nodes, MigxnContext context, ILGenerator generator)
    {
        Identifier? identifier = nodes[0] as Identifier;
        Debug.Assert(identifier != null);
        switch (nodes[1])
        {
            case EqlToken:
                // 获取变量
                MigxnVariable variable = context.Variables.LoadVariable(Text.ToString());
                MigxnExpr? expr = nodes[2] as MigxnExpr;
                Debug.Assert(expr != null);
                VarToken.StoreLocal(context, generator, variable, expr, Position);
                return;
            default:
                throw new NotStatementException(Position);
        }
    }

    public static MigxnToken Create(int position, string input) => input switch
    {
        "var" => new VarToken(position),
        _ => new Identifier(position, input.AsMemory())
    };
    
    internal override void EmitCode(MigxnContext context, ILGenerator generator)
    {
        MigxnSymbols<MigxnVariable> table = context.Variables;
        MigxnVariable variable = table.LoadVariable(Text.ToString());
        variable.LoadVar(generator);
    }

    internal override Type ExpressionType(MigxnContext context) 
        => context.Variables.LoadVariable(Text.ToString()).Type;
}