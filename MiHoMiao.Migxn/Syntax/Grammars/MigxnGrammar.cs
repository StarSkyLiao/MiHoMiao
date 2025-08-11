using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
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
    
    internal bool TryMatchNext<T>([MaybeNullWhen(false)]out T result) where T : MigxnNode?
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
            MigxnTree? buildTree = ParseNext();
            if (buildTree != null) yield return buildTree;
        }
    }

    internal MigxnTree? ParseNext(bool fallback = false)
    {
        int startIndex = m_Index;
        if (Current is ILeadToken leadToken)
        {
            Result<MigxnTree> result = leadToken.TryCollectToken(this);
            if (result.IsSuccess) return result.Value;
            m_Exceptions.Add(result.Exception!);
            if (fallback) m_Index = startIndex;
            return (result.Exception as IBadTreeException)?.MigxnTree;
        }
        if (Current is LiteralToken literalToken)
        {
            MoveNext();
            return new TokenExpr(literalToken);
        }
        if (fallback) m_Index = startIndex;
        else MoveNext();
        return null;

    }
}