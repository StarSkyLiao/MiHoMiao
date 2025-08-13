using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

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
    private readonly List<Exception> m_Exceptions;
    
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

    public IEnumerable<Exception> Exceptions => m_Exceptions;
    
    public IEnumerable<MigxnTree> MigxnTrees => m_MigxnTrees;

    private IEnumerable<MigxnTree> BuildTree()
    {
        m_Index = 0;
        while (Current != null)
        {
            MigxnTree? result = ForceParseTree<MigxnTree>();
            if (result is not null) yield return result;
        }
    }
    
    internal bool TryParseTree<T>([MaybeNullWhen(false)]out T result) where T : MigxnTree?
    {
        int startIndex = m_Index;
        Result<MigxnTree> next = ParseNext<T>();
        result = next.Value as T;
        if (result is not null) return true;
        m_Index = startIndex; 
        return false;
    }
    
    internal MigxnTree? ForceParseTree<T>() where T : MigxnTree?
    {
        Result<MigxnTree> result = ParseNext<T>();
        if (result is { IsSuccess: true, Value: T next }) return next;
        m_Exceptions.Add(result.IsSuccess ? new TokenMissingException(new BadTree([result]), typeof(T).Name) : result.Exception!);
        return null;
    }
    
    private Result<MigxnTree> ParseNext<T>() where T : MigxnTree?
    {
        if (Current is ILeadToken leadToken)
        {
            Result<MigxnTree> result = leadToken.TryCollectToken(this);
            if (result is { IsSuccess: true, Value: T next }) return next;
            return result.IsSuccess ? new TokenMissingException(new BadTree([result]), typeof(T).Name) : result.Exception!;
        }
        if (Current is LiteralToken literalToken)
        {
            MoveNext();
            TokenExpr tokenExpr = new TokenExpr(literalToken);
            if (tokenExpr is T next) return next;
            BadTree badTree = new BadTree([tokenExpr]);
            return new TokenMissingException(badTree, typeof(T).Name);
        }
        MoveNext();
        return new Result<MigxnTree>(value:null!);
    }
    
}