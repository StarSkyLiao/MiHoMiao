// using System.Diagnostics;
// using System.Reflection.Emit;
// using MiHoMiao.Core.Collections.Tool;
// using MiHoMiao.Migxn.Exceptions.Grammar;
// using MiHoMiao.Migxn.Reflection;
// using MiHoMiao.Migxn.Runtime;
// using MiHoMiao.Migxn.Syntax.Node;
// using MiHoMiao.Migxn.Syntax.Node.Expressions;
// using MiHoMiao.Migxn.Syntax.Node.Statements;
// using MiHoMiao.Migxn.Syntax.Tokens.Literals;
// using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;
//
// namespace MiHoMiao.Migxn.Syntax.Tokens.Keywords;
//
// public record IfToken(int Position) : MigxnToken(Position, "if".AsMemory()), IKeyword
// {
//     StatementNode IKeyword.CollectNode(MigxnParser parser)
//     {
//         List<MigxnNode> nodes = new List<MigxnNode>(5) { this, parser.Expect<Identifier>() };
//         switch (parser.Current)
//         {
//             case ColonToken:
//             {
//                 nodes.Add(parser.Expect<ColonToken>());
//                 nodes.Add(parser.Expect<Identifier>());
//                 nodes.Add(parser.Expect<EqlToken>());
//                 MigxnExpr? expression = parser.ParseExpression(0, false);
//                 if (expression != null) nodes.Add(expression);
//                 break;
//             }
//             case EqlToken:
//             {
//                 nodes.Add(parser.Expect<EqlToken>());
//                 MigxnExpr? initValue = parser.ParseExpression(0, false);
//                 if (initValue is null) throw new NotSupportedException($"Position {Position}: Type is not clearly.");
//                 nodes.Add(initValue);
//                 break;
//             }
//             default:
//                 throw new NotSupportedException($"Position {Position}: Var grammar is not in correct format.");
//         }
//
//         return new StatementNode(nodes,
//             nodes.GenericViewer(item => item.Text.ToString(),
//                 "", "", " ").AsMemory()
//         );
//     }
//
//
//     void IKeyword.EmitCode(List<MigxnNode> nodes, MigxnContext context, ILGenerator generator)
//     {
//         Identifier? identifier = nodes[1] as Identifier;
//         Debug.Assert(identifier != null);
//         
//         switch (nodes[2])
//         {
//             case ColonToken:
//             {
//                 // 验证类型
//                 Identifier? typeToken = nodes[3] as Identifier;
//                 Debug.Assert(typeToken != null);
//                 string typeName = typeToken.Text.ToString();
//                 Type? type = ReflectTool.LoadType(typeName);
//                 if (type == null) throw new TypeNotFoundException(typeToken.Position);
//                 // 获取变量
//                 MigxnSymbols<MigxnVariable> table = context.Variables;
//                 MigxnVariable variable = new MigxnVariable(type, table.Count);
//                 // 如果有可能, 则赋值变量
//                 if (nodes.Count < 6 || nodes[4] is not EqlToken) return;
//                 MigxnExpr? expr = nodes[5] as MigxnExpr;
//                 Debug.Assert(expr != null);
//                 // 类型不匹配
//                 Type exprType = expr.ExpressionType(context);
//                 var result = TypeTool.IsTypeConvertible(Position, type, exprType);
//                 if (!result.IsSuccess) throw result.Exception!;
//                 expr.EmitCode(context, generator);
//                 result.Value?.Invoke(generator);
//                 variable.StoreVar(context, generator);
//                 // 声明变量
//                 table.DeclareVariable(identifier.Text.ToString(), variable);
//                 return;
//             }
//             case EqlToken:
//             {
//                 // 获取变量
//                 MigxnSymbols<MigxnVariable> table = context.Variables;
//                 // 如果有可能, 则赋值变量
//                 MigxnExpr? expr = nodes[3] as MigxnExpr;
//                 Debug.Assert(expr != null);
//                 expr.EmitCode(context, generator);
//                 MigxnVariable variable = new MigxnVariable(expr.ExpressionType(context), table.Count);
//                 variable.StoreVar(context, generator);
//                 // 声明变量
//                 table.DeclareVariable(identifier.Text.ToString(), variable);
//                 return;
//             }
//             default:
//                 throw new NotSupportedException($"Position {Position}: Type of {identifier.Text} is not clearly.");
//         }
//     }
//     
// }