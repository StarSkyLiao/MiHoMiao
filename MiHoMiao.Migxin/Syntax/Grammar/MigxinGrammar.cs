using System.Collections.Immutable;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.Syntax.Grammar.Expr;
using MiHoMiao.Migxin.Syntax.Lexical;
using MiHoMiao.Migxin.Syntax.Lexical.Trivia;

namespace MiHoMiao.Migxin.Syntax.Grammar;

internal class MigxinGrammar
{
    private MigxinGrammar(MigxinLexer input)
    {
        m_Tokens = [..input.MigxnTokens.Where(item => item is not TriviaToken)];
        m_Exceptions = [..input.Exceptions];
    }
        
    public static MigxinGrammar Parse(MigxinLexer input)
    {
        MigxinGrammar result = new MigxinGrammar(input);
        result.m_MigxinTrees.AddRange(result.BuildTree());
        return result;
    }
    
    #region Information
    
    private readonly ImmutableArray<MigxinToken> m_Tokens;
    
    /// <summary>
    /// 当前的 Token 索引
    /// </summary>
    private int m_Index;
    
    private readonly List<DiagnosticBag> m_Exceptions = [];

    private readonly List<MigxinTree> m_MigxinTrees = [];
    
    /// <summary>
    /// 解析过程中的异常
    /// </summary>
    public IList<DiagnosticBag> Exceptions => m_Exceptions;
    
    /// <summary>
    /// 解析到的 Token.
    /// </summary>
    public IList<MigxinTree> MigxinTrees => m_MigxinTrees;
    
    #endregion
    
    #region Parsing
    
    internal MigxinToken? Current => m_Index >= m_Tokens.Length ? null : m_Tokens[m_Index];
    
    internal MigxinToken? Next => m_Index + 1 >= m_Tokens.Length ? null : m_Tokens[m_Index + 1];

    internal (int, int) Position => (m_Index >= m_Tokens.Length ? m_Tokens[^1] : m_Tokens[m_Index]).Position;
    
    internal MigxinToken? Peek(int offset)
    {
        int index = m_Index + offset;
        return index >= m_Tokens.Length ? null : m_Tokens[index];
    }
    
    internal MigxinToken? MoveNext()
    {
        int index = m_Index++;
        if (index >= m_Tokens.Length) return null;
        MigxinToken token = m_Tokens[index];
        return token;
    }    
    
    #endregion

    private IEnumerable<MigxinTree> BuildTree()
    {
        m_Index = 0;
        while (Current != null)
        {
            MigxinResult<MigxinExpr> result = MigxinExpr.TryParse(this);
            if (result.IsSuccess) yield return result.Result!;
            else
            {
                MoveNext();
                m_Exceptions.Add(result.Exception!);
            }
        }
    }
    
    // internal IResult<MigxnStmt> ParseStmt()
    // {
    //     if (Current is ILeadToken leadToken) return leadToken.TryCollectToken(this);
    //     IResult<MigxnExpr> pointer = MigxnExpr.ParseUnitExpr(this);
    //     if (!pointer.IsSuccess) return new Diagnostic<MigxnStmt>(pointer.Exception!);
    //     Debug.Assert(pointer.Result != null);
    //     
    //     MigxinToken? token = MoveNext();
    //     if (token is AssignToken)
    //     {
    //         IResult<MigxnExpr> expr = MigxnExpr.ParseUnitExpr(this);
    //         if (!expr.IsSuccess) return new Diagnostic<MigxnStmt>(expr.Exception!);
    //         Debug.Assert(expr.Result != null);
    //         AssignStmt assign = new AssignStmt(pointer.Result, expr.Result);
    //         return new Diagnostic<MigxnStmt>(assign);
    //     }
    //
    //     return new Diagnostic<MigxnStmt>(new BadAssignment(new BadTree([pointer.Result, token])));
    // }
    //

    //
    // public T? TryMatchToken<T>() where T : MigxinToken
    // {
    //     if (Current is not T token) return null;
    //     MoveNext();
    //     return token;
    // }
    //
    // public string CodeFormat()
    // {
    //     using InterpolatedString formatCode = new InterpolatedString(512);
    //     foreach (MigxnTree migxnTree in MigxnTrees)
    //     {
    //         formatCode.Append(migxnTree.Text);
    //         formatCode.Append('\n');
    //     }
    //
    //     return formatCode.ToString();
    // }
    
}