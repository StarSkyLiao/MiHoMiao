using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using MiHoMiao.Core.Numerics.Values;
using MiHoMiao.Core.Reflection;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Lexical;
using MiHoMiao.Migxin.Syntax.Lexical.Literals;
using MiHoMiao.Migxin.Syntax.Lexical.Trivia;

namespace MiHoMiao.Migxin.Syntax.Lexical;

public class MigxinLexer
{
    // [field: AllowNull, MaybeNull]
    // private static IDictionary<string, Func<int, (int, int), AbstractKeyword>> Keyword => field ??=
    //     new Dictionary<string, Func<int, (int, int), AbstractKeyword>>(
    //         from type in typeof(MigxnLexer).Assembly.GetTypes()
    //         where type.IsAssignableTo(typeof(AbstractKeyword)) && type.IsAssignableTo(typeof(IKeywordToken)) && !type.IsAbstract
    //         select new KeyValuePair<string, Func<int, (int, int), AbstractKeyword>>(
    //             (string)type.GetProperty(nameof(IKeywordToken.UniqueName)).GetValue(null),
    //             type.GetMethod(nameof(IKeywordToken.Create)).CreateDelegate<Func<int, (int, int), AbstractKeyword>>(null)
    //         )
    //     );
    //
    // [field: AllowNull, MaybeNull]
    // private static IDictionary<string, Func<int, (int, int), MigxnOperator>> Operator => field ??=
    //     new Dictionary<string, Func<int, (int, int), MigxnOperator>>(
    //         from type in typeof(MigxnLexer).Assembly.GetTypes()
    //         where type.IsAssignableTo(typeof(MigxnOperator)) && type.IsAssignableTo(typeof(IOperatorToken)) && !type.IsAbstract
    //         select new KeyValuePair<string, Func<int, (int, int), MigxnOperator>>(
    //             (string)type.GetProperty(nameof(IOperatorToken.UniqueName)).GetValue(null),
    //             type.GetMethod(nameof(IOperatorToken.Create)).CreateDelegate<Func<int, (int, int), MigxnOperator>>(null)
    //         )
    //     );

    #region Information
    
    private MigxinLexer(string input) => m_Input = input;

    /// <summary>
    /// 当前的字符串索引
    /// </summary>
    internal int CharIndex;

    /// <summary>
    /// 当前的行号
    /// </summary>
    internal int LineNumber = 1;

    /// <summary>
    /// 当前的列号
    /// </summary>
    internal int ColumnNumber = 1;
    
    /// <summary>
    /// 存储解析过程中的异常
    /// </summary>
    private readonly List<DiagnosticBag> m_Exceptions = [];
    
    /// <summary>
    /// 解析到的 Token.
    /// </summary>
    private readonly List<MigxinToken> m_MigxnTokens = [];

    private readonly string m_Input;

    private (int, (int, int)) m_Frame;
    
    internal (int, int) Position => (LineNumber, ColumnNumber);

    internal (int, (int, int)) CreateFrame() => m_Frame = (CharIndex, (LineNumber, ColumnNumber));

    internal void RestoreFrame() => (CharIndex, (LineNumber, ColumnNumber)) = m_Frame;
    
    internal char Current => CharIndex >= m_Input.Length ? '\0' : m_Input[CharIndex];

    internal char Next => CharIndex + 1 >= m_Input.Length ? '\0' : m_Input[CharIndex + 1];
    
    internal char Peek(int offset)
    {
        int index = CharIndex + offset;
        return index >= m_Input.Length ? '\0' : m_Input[index];
    }
    
    /// <summary>
    /// 向前移动 1 步并返回该字符
    /// </summary>
    internal char MoveNext()
    {
        int index = CharIndex++;
        if (index >= m_Input.Length) return '\0';
        char c = m_Input[index];
        if (c == '\n')
        {
            LineNumber++;
            ColumnNumber = 1;
        }
        else
        {
            ColumnNumber++;
        }
        return c;
    }
    
    /// <summary>
    /// 向前移动 count 步
    /// </summary>
    internal void MoveAhead(int count)
    {
        for (int i = 0; i < count; i++) MoveNext();
    }
    
    /// <summary>
    /// 返回 [start, tail) 之间的字符跨度
    /// </summary>
    internal ReadOnlyMemory<char> AsMemory(int start, int tail) => m_Input.AsMemory()[start..tail.Max(m_Input.Length)];
    
    /// <summary>
    /// 返回 [start, tail) 之间的字符跨度
    /// </summary>
    internal ReadOnlySpan<char> AsSpan(int start, int tail) => m_Input.AsSpan()[start..tail.Max(m_Input.Length)];

    private IEnumerable<MigxinToken> Lex()
    {
        while (char.IsWhiteSpace(Current)) MoveNext();
        while (Current != '\0')
        {
            IEnumerable<MigxinToken?> matchResult = MigxinToken.TokenParsers.Select(func => func(this));
            MigxinToken? result = matchResult.OfType<MigxinToken>().FirstOrDefault();
            if (result is not null) yield return result;
            else Exceptions.Add(new UnknownToken(Position, MoveNext().ToString()));
            while (char.IsWhiteSpace(Current)) MoveNext();
        }
    }
    
    #endregion
    
    public static MigxinLexer Parse(string input)
    {
        MigxinLexer result = new MigxinLexer(input);
        result.m_MigxnTokens.AddRange(result.Lex());
        return result;
    }

    public IList<DiagnosticBag> Exceptions => m_Exceptions;
    
    public IList<MigxinToken> MigxnTokens => m_MigxnTokens;
    
}