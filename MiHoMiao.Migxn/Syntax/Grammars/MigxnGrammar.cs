using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Lexers;

namespace MiHoMiao.Migxn.Syntax.Grammars;

public class MigxnGrammar
{
    
    #region Information

    private MigxnGrammar(MigxnLexer lexer)
    {
        m_Tokens = [..lexer.MigxnTokens];
        m_Exceptions = [..lexer.Exceptions];
    }
    
    private readonly ImmutableArray<MigxnToken> m_Tokens;
    
    /// <summary>
    /// 当前的 Token 索引
    /// </summary>
    private int m_Index;

    /// <summary>
    /// 存储解析过程中的异常
    /// </summary>
    private readonly List<BadMigxnTree> m_Exceptions;
    
    private readonly List<MigxnTree> m_MigxnTrees = [];

    internal MigxnToken? Current => m_Index >= m_Tokens.Length ? null : m_Tokens[m_Index];

    internal MigxnToken? Next => m_Index + 1 >= m_Tokens.Length ? null : m_Tokens[m_Index + 1];
    
    internal MigxnToken? Peek(int offset)
    {
        int index = m_Index + offset;
        return index >= m_Tokens.Length ? null : m_Tokens[index];
    }
    
    internal bool TryMatchToken<T>([MaybeNullWhen(false)]out T result) where T : MigxnNode?
    {
        result = Current as T;
        if (result is null) return false;
        MoveNext();
        return true;
    }
    
    internal MigxnToken? MoveNext()
    {
        int index = m_Index++;
        if (index >= m_Tokens.Length) return null;
        MigxnToken token = m_Tokens[index];
        return token;
    }
    
    internal void MoveNext(int count)
    {
        for (int i = 0; i < count; i++) MoveNext();
    }

    #endregion
    
    public static MigxnGrammar Parse(MigxnLexer lexer)
    {
        MigxnGrammar result = new MigxnGrammar(lexer);
        result.m_MigxnTrees.AddRange(result.BuildTree());
        return result;
    }

    public IEnumerable<BadMigxnTree> Exceptions => m_Exceptions;
    
    public IEnumerable<MigxnTree> MigxnTrees => m_MigxnTrees;

    private IEnumerable<MigxnTree> BuildTree()
    {
        m_Index = 0;
        while (Current != null)
        {
            IResult<MigxnExpr> result = TryParse<MigxnExpr>();
            if (result.IsSuccess) yield return result.Result!;
            else m_Exceptions.Add(result.Exception!);
        }
    }
    
    public IResult<T> TryParse<T>() where T : class, IExprParser<T> => T.TryParse(this);
    
    public T? TryMatchToken<T>() where T : MigxnToken
    {
        if (Current is not T token) return null;
        MoveNext();
        return token;
    }
    //
    // internal bool TryParseTree<T>([MaybeNullWhen(false)]out T result) where T : MigxnTree?
    // {
    //     int startIndex = m_Index;
    //     Result<MigxnTree> next = ParseNext<T>();
    //     result = next.Value as T;
    //     if (result is not null) return true;
    //     m_Index = startIndex; 
    //     return false;
    // }
    //
    // private Result<MigxnTree> ParseNext<T>() where T : MigxnTree?
    // {
    //     if (Current is ILeadToken leadToken)
    //     {
    //         Result<MigxnTree> result = leadToken.TryCollectToken(this);
    //         if (result is { IsSuccess: true, Value: T next }) return next;
    //         return result.IsSuccess ? new SpecifiedTokenMissing(new BadTree([result]), typeof(T).Name) : result.Exception!;
    //     }
    //     if (Current is LiteralToken literalToken)
    //     {
    //         MoveNext();
    //         TokenExpr tokenExpr = new TokenExpr(literalToken);
    //         if (tokenExpr is T next) return next;
    //         BadTree badTree = new BadTree([tokenExpr]);
    //         return new SpecifiedTokenMissing(badTree, typeof(T).Name);
    //     }
    //     MoveNext();
    //     return new Result<MigxnTree>(value:null!);
    // }
    
}