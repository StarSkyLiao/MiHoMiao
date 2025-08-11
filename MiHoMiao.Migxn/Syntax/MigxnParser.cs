using MiHoMiao.Migxn.Exceptions.Grammar;
using MiHoMiao.Migxn.Syntax.Node;
using MiHoMiao.Migxn.Syntax.Node.Expressions;
using MiHoMiao.Migxn.Syntax.Node.Statements;
using MiHoMiao.Migxn.Syntax.Tokens;
using MiHoMiao.Migxn.Syntax.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

namespace MiHoMiao.Migxn.Syntax;

public class MigxnParser()
{
    internal readonly List<MigxnToken> Tokens = [];
    internal int CurrIndex;

    #region 入口
    public MigxnTree? Parse(IEnumerable<MigxnToken> tokens)
    {
        Tokens.Clear();
        Tokens.AddRange(tokens);
        CurrIndex = 0;
        List<MigxnNode> nodes = [..ParseStatement()];
        return nodes.Count > 0 ? new MigxnTree(nodes) : null;
    }
    #endregion

    #region 核心 Pratt 解析

    internal IEnumerable<StatementNode> ParseStatement()
    {
        int depth = 0;
        while (Current != null)
        {
            switch (Current)
            {
                case CurlyRightToken:
                {
                    MoveNext();
                    yield break;
                }
                case CurlyLeftToken:
                {
                    ++depth;
                    MoveNext();
                    yield return new BlockStatement(ParseStatement().ToList(), "{}".AsMemory());
                    if (--depth < 0) throw new NotStatementException(Current.Position);
                    break;
                }
                case IKeyword keyword:
                    MoveNext();
                    yield return keyword.CollectNode(this);
                    break;
                default:
                    throw new NotStatementException(Current.Position);
            }
        }
        
        if (depth > 0) throw new NotStatementException(0);
    }

    
    internal MigxnExpr? ParseExpression(int minPrec, bool isInCurly)
    {
        MigxnExpr? left = ParsePrefix();
        if (left is null) return null;
        while (true)
        {
            IBinaryToken? op = Current as IBinaryToken;
            if (op == null || op.BinaryOpPriority < minPrec) break;
            MoveNext();
            
            MigxnExpr? right = ParseExpression(op.BinaryOpPriority + 1, false);
            if (right is null) throw new NotSupportedException($"Except for expression, but found null!");
            left = new BinaryExpr(left, op, right, isInCurly);
        }
        return left;
    }
    
    private MigxnExpr? ParsePrefix()
    {
        switch (Current)
        {
            // 括号
            case RoundRightToken:
            {
                MoveNext();
                MigxnExpr? expr = ParseExpression(0, true);
                Expect<RoundLeftToken>();
                return expr;
            }
            // 一元前缀运算符
            case IUnaryToken u:
            {
                MoveNext();
                MigxnExpr? operand = ParsePrefix();
                if(operand is null) throw new NotSupportedException($"Except for expression, but found null!");
                return new UnaryExpr(u, operand);
            }
            // 字面量
            case LiteralToken lit:
                MoveNext();
                return new LiteralExpr(lit) ;
            default:
                return null;
        }
    }
    #endregion

    #region 工具
    internal MigxnToken? Current => CurrIndex < Tokens.Count ? Tokens[CurrIndex] : null;

    internal void MoveNext() => CurrIndex++;
    
    internal MigxnToken? Peek(int offset) => (CurrIndex + offset) < Tokens.Count ? Tokens[CurrIndex + offset] : null;
    
    /// <summary>
    /// 如果当前的 令牌 满足类型 T, 则执行 MoveNext();
    /// 否则, 抛出异常.
    /// </summary>
    internal T Expect<T>() where T : MigxnNode
    {
        if (Current is not T result) throw new Exception($"Expected '{typeof(T).Name}', but got {Current}");
        MoveNext();
        return result;
    }
    #endregion
}
