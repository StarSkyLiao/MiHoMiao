using System.Diagnostics;
using System.Reflection.Emit;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.Exceptions.Grammar;
using MiHoMiao.Migxn.Reflection;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Node;
using MiHoMiao.Migxn.Syntax.Node.Expressions;
using MiHoMiao.Migxn.Syntax.Node.Statements;
using MiHoMiao.Migxn.Syntax.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

namespace MiHoMiao.Migxn.Syntax.Tokens.Keywords;

public record VarToken(int Position) : MigxnToken(Position, "var".AsMemory()), IKeyword
{
    StatementNode IKeyword.CollectNode(MigxnParser parser)
    {
        List<MigxnNode> nodes = new List<MigxnNode>(5) { this, parser.Expect<Identifier>() };
        switch (parser.Current)
        {
            case ColonToken:
            {
                nodes.Add(parser.Expect<ColonToken>());
                nodes.Add(parser.Expect<Identifier>());
                nodes.Add(parser.Expect<EqlToken>());
                MigxnExpr? expression = parser.ParseExpression(0, false);
                if (expression != null) nodes.Add(expression);
                break;
            }
            case EqlToken:
            {
                nodes.Add(parser.Expect<EqlToken>());
                MigxnExpr? initValue = parser.ParseExpression(0, false);
                if (initValue is null) throw new NotSupportedException($"Position {Position}: Type is not clearly.");
                nodes.Add(initValue);
                break;
            }
            default:
                throw new NotSupportedException($"Position {Position}: Var grammar is not in correct format.");
        }

        return new StatementNode(nodes,
            nodes.GenericViewer(item => item.Text.ToString(),
                "", "", " ").AsMemory()
        );
    }


    void IKeyword.EmitCode(List<MigxnNode> nodes, MigxnContext context, ILGenerator generator)
    {
        Identifier? identifier = nodes[1] as Identifier;
        Debug.Assert(identifier != null);
        string varName = identifier.Text.ToString();
        
        switch (nodes[2])
        {
            case ColonToken:
            {
                // 验证类型
                Identifier? typeToken = nodes[3] as Identifier;
                Debug.Assert(typeToken != null);
                string typeName = typeToken.Text.ToString();
                Type? type = ReflectTool.LoadType(typeName);
                if (type == null) throw new TypeNotFoundException(typeToken.Position);
                // 获取变量. 如果有可能, 则赋值变量
                MigxnExpr? expr = (nodes.Count < 6 || nodes[4] is not EqlToken) ? null : nodes[5] as MigxnExpr;
                MigxnVariable variable = new MigxnVariable(varName, type);
                DeclareLocal(context, generator, variable, expr, Position);
                return;
            }
            case EqlToken:
            {
                // 如果有可能, 则赋值变量
                MigxnExpr? expr = nodes[3] as MigxnExpr;
                Debug.Assert(expr != null);
                MigxnVariable variable = new MigxnVariable(varName, expr.ExpressionType(context));
                DeclareLocal(context, generator, variable, expr, Position);
                return;
            }
            default:
                throw new NotSupportedException($"Position {Position}: Type of {identifier.Text} is not clearly.");
        }
    }

    internal static void DeclareLocal(MigxnContext context, ILGenerator generator, 
        MigxnVariable variable, MigxnExpr? expr, int position)
    {
        if (expr is not null)
        {
            // 类型不匹配, 则抛出异常
            Type exprType = expr.ExpressionType(context);
            var result = TypeTool.IsTypeConvertible(position, variable.Type, exprType);
            if (!result.IsSuccess) throw result.Exception!;
            // 首先, 生成表达式的代码
            expr.EmitCode(context, generator);
            result.Value?.Invoke(generator);
        }
        // 然后, 声明局部变量并赋值
        context.Variables.DeclareVariable(variable.Name, variable);
        LocalBuilder localBuilder = generator.DeclareLocal(variable.Type);
        variable.LocalBuilder = localBuilder;
        variable.StoreVar(generator);
    }
    
    internal static void StoreLocal(MigxnContext context, ILGenerator generator, 
        MigxnVariable variable, MigxnExpr expr, int position)
    {
        // 类型不匹配, 则抛出异常
        Type exprType = expr.ExpressionType(context);
        var result = TypeTool.IsTypeConvertible(position, variable.Type, exprType);
        if (!result.IsSuccess) throw result.Exception!;
        // 然后, 生成表达式的代码
        expr.EmitCode(context, generator);
        result.Value?.Invoke(generator);
            
        // 最后, 赋值
        variable.StoreVar(generator);
    }
    
}