using System.Collections.Immutable;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.Compiler.Semantic.Stmt;
using MiHoMiao.Migxin.Compiler.Syntax;

namespace MiHoMiao.Migxin.Compiler.Semantic;

internal class MigxinParser
{
    private MigxinParser(MigxinGrammar input)
    {
        m_Nodes = [..input.MigxinNodes];
        m_Exceptions = [..input.Exceptions];
    }

    public static MigxinParser Parse(MigxinGrammar input) => new MigxinParser(input);
    
    #region Information

    private int m_Index;
    
    private readonly ImmutableArray<MigxinNode> m_Nodes;
    
    private readonly List<DiagnosticBag> m_Exceptions;
    
    /// <summary>
    /// 解析过程中的异常
    /// </summary>
    public IList<MigxinNode> MigxinNodes => m_Nodes;
    
    /// <summary>
    /// 解析过程中的异常
    /// </summary>
    public IList<DiagnosticBag> Exceptions => m_Exceptions;

    /// <summary>
    /// 解析过程中的警告
    /// </summary>
    public List<DiagnosticBag> Warnings = [];

    /// <summary>
    /// 解析过程中的建议
    /// </summary>
    public List<DiagnosticBag> Tips = [];
    
    #endregion

    internal void ParseForward()
    {
        
    }
    
    internal void ParseNode(MigxinNode node)
    {
        Type? type = node.GetType();
        ITreeVisitor.Visitor? visitor;
        do
        {
            visitor = ITreeVisitor.TreeVisitors.GetValueOrDefault(type);
            type = type.BaseType;
        } while (type != null && visitor == null);
        visitor?.Invoke(this, node);
    }
    
}