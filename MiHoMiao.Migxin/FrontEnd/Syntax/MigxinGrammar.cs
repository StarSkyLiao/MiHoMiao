using System.Collections.Immutable;
using System.Diagnostics;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Grammar;
using MiHoMiao.Migxin.FrontEnd.Lexical;
using MiHoMiao.Migxin.FrontEnd.Lexical.Trivia;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Binary;
using MiHoMiao.Migxin.FrontEnd.Syntax.Stmt;

namespace MiHoMiao.Migxin.FrontEnd.Syntax;

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
        IEnumerable<MigxinNode> migxinNodes = result.BuildTree();
        result.m_MigxinTrees.AddRange(migxinNodes);
        return result;
    }
    
    #region Information
    
    private readonly ImmutableArray<MigxinToken> m_Tokens;
    
    /// <summary>
    /// 当前的 Token 索引
    /// </summary>
    private int m_Index;
    
    private readonly List<DiagnosticBag> m_Exceptions = [];

    private readonly List<MigxinNode> m_MigxinTrees = [];
    
    /// <summary>
    /// 解析过程中的异常
    /// </summary>
    public IList<DiagnosticBag> Exceptions => m_Exceptions;
    
    /// <summary>
    /// 解析到的 Token.
    /// </summary>
    public IList<MigxinNode> MigxinTrees => m_MigxinTrees;
    
    #endregion
    
    #region Parsing

    private int m_Frame;
    
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
    
    internal T? TryMatchToken<T>() where T : MigxinToken
    {
        if (Current is not T token) return null;
        MoveNext();
        return token;
    }
    
    internal int CreateFrame() => m_Frame = m_Index;

    internal void RestoreFrame() => m_Index = m_Frame;
    
    #endregion

    private IEnumerable<MigxinNode> BuildTree()
    {
        m_Index = 0;
        while (Current != null)
        {
            MigxinResult<MigxinStmt> migxinNode = ParseStmt();
            if (migxinNode.IsSuccess) yield return migxinNode.Result!;
            else m_Exceptions.Add(migxinNode.Exception!);
        }
    }

    private MigxinResult<MigxinStmt> ParseStmt()
    {
        MigxinToken? current = Current;
        Debug.Assert(current != null);
        ITokenGrammar.StmtParser? parser = MigxinStmt.StmtParsers.GetValueOrDefault(current.GetType());
        if (parser is null)
        {
            MigxinResult<MigxinExpr> expr = MigxinExpr.TryParse(this);
            if (!expr.IsSuccess) return expr.Exception!;

            if (expr.Result is BinaryExpr { OperatorSymbol: AssignSymbol assignSymbol } binaryExpr)
                return new AssignStmt(binaryExpr.Left, assignSymbol, binaryExpr.Right);

            return new MigxinResult<MigxinStmt>(new NotStmt(expr.Result!.Position));
        }
        MigxinResult<MigxinStmt> result = parser(this);
        if (current == Current) MoveNext();
        return result;
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