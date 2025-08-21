using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Lexer;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Comments;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers;

internal class MigxnLexer
{

    [field: AllowNull, MaybeNull]
    private static IDictionary<string, Func<int, (int, int), AbstractKeyword>> Keyword => field ??=
        new Dictionary<string, Func<int, (int, int), AbstractKeyword>>(
            from type in typeof(MigxnLexer).Assembly.GetTypes()
            where type.IsAssignableTo(typeof(AbstractKeyword)) && type.IsAssignableTo(typeof(IKeywordToken)) && !type.IsAbstract
            select new KeyValuePair<string, Func<int, (int, int), AbstractKeyword>>(
                (string)type.GetProperty(nameof(IKeywordToken.UniqueName)).GetValue(null),
                type.GetMethod(nameof(IKeywordToken.Create)).CreateDelegate<Func<int, (int, int), AbstractKeyword>>(null)
            )
        );
    
    [field: AllowNull, MaybeNull]
    private static IDictionary<string, Func<int, (int, int), MigxnOperator>> Operator => field ??=
        new Dictionary<string, Func<int, (int, int), MigxnOperator>>(
            from type in typeof(MigxnLexer).Assembly.GetTypes()
            where type.IsAssignableTo(typeof(MigxnOperator)) && type.IsAssignableTo(typeof(IOperatorToken)) && !type.IsAbstract
            select new KeyValuePair<string, Func<int, (int, int), MigxnOperator>>(
                (string)type.GetProperty(nameof(IOperatorToken.UniqueName)).GetValue(null),
                type.GetMethod(nameof(IOperatorToken.Create)).CreateDelegate<Func<int, (int, int), MigxnOperator>>(null)
            )
        );

    #region Information
    
    private MigxnLexer(string input) => m_Input = input;

    /// <summary>
    /// 当前的字符串索引
    /// </summary>
    private int m_Index;

    /// <summary>
    /// 当前的行号
    /// </summary>
    private int m_LineNumber = 1;

    /// <summary>
    /// 当前的列号
    /// </summary>
    private int m_ColumnNumber = 1;
    
    /// <summary>
    /// 存储解析过程中的异常
    /// </summary>
    private readonly List<BadMigxnTree> m_Exceptions = [];
    
    /// <summary>
    /// 解析到的 Token.
    /// </summary>
    private readonly List<MigxnToken> m_MigxnTokens = [];

    private readonly string m_Input;

    private char Current => m_Index >= m_Input.Length ? '\0' : m_Input[m_Index];

    private char Next => m_Index + 1 >= m_Input.Length ? '\0' : m_Input[m_Index + 1];
    
    private char Peek(int offset)
    {
        int index = m_Index + offset;
        return index >= m_Input.Length ? '\0' : m_Input[index];
    }
    
    private char MoveNext()
    {
        int index = m_Index++;
        if (index >= m_Input.Length) return '\0';
        char c = m_Input[index];
        if (c == '\n')
        {
            m_LineNumber++;
            m_ColumnNumber = 1;
        }
        else
        {
            m_ColumnNumber++;
        }
        return c;
    }
    
    private void MoveNext(int count)
    {
        for (int i = 0; i < count; i++) MoveNext();
    }

    #endregion

    public static MigxnLexer Parse(string input)
    {
        MigxnLexer result = new MigxnLexer(input);
        result.m_MigxnTokens.AddRange(result.Lex());
        return result;
    }

    public IEnumerable<BadMigxnTree> Exceptions => m_Exceptions;
    
    public IEnumerable<MigxnToken> MigxnTokens => m_MigxnTokens;
    
    private IEnumerable<MigxnToken> Lex()
    {
        SkipWhiteSpace();
        while (Current != '\0')
        {
            int startIndex = m_Index;
            int startLine = m_LineNumber;
            int startColumn = m_ColumnNumber;

            if (Current == '#')
            {
                if (Peek(1) == '#' && Peek(2) == '#')
                {
                    yield return ReadMultiLineComment(startIndex, startLine, startColumn);
                }
                else
                {
                    yield return ReadSingleLineComment(startIndex, startLine, startColumn);
                }
            }
            else if (char.IsDigit(Current))
            {
                yield return ReadNumber(startIndex, startLine, startColumn);
            }
            else if (Current == '"')
            {
                yield return ReadString(startIndex, startLine, startColumn);
            }
            else if (Current == '\'')
            {
                yield return ReadChar(startIndex, startLine, startColumn);
            }
            else if (IsOperatorChar(Current))
            {
                yield return ReadOperator(startIndex, startLine, startColumn);
            }
            else if (char.IsLetter(Current) || Current == '_')
            {
                yield return ReadIdentifier(startIndex, startLine, startColumn);
            }
            else
            {
                string invalid = MoveNext().ToString();
                BadToken badToken = new BadToken(invalid.AsMemory(), startIndex, (startLine, startColumn));
                m_Exceptions.Add(new UnrecognizedToken(badToken));
            }
            SkipWhiteSpace();
        }
    }
    
    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(Current)) MoveNext();
    }

    private SingleLineComment ReadSingleLineComment(int startIndex, int line, int column)
    {
        int start = m_Index;
        while (Current != '\0' && Current != '\n') MoveNext();
        return new SingleLineComment(startIndex, m_Input.AsMemory()[start..m_Index], (line, column));
    }

    private MultiLineComment ReadMultiLineComment(int startIndex, int line, int column)
    {
        int start = m_Index;
        MoveNext(3);
        while (Current != '\0')
        {
            if (Current != '#' || Peek(1) != '#' || Peek(2) != '#')
            {
                MoveNext();
                continue;
            }
            MoveNext(3);
            break;
        }
        return new MultiLineComment(startIndex, m_Input.AsMemory()[start..m_Index], (line, column));
    }
    
    private LiteralToken ReadNumber(int startIndex, int line, int column)
    {
        int start = m_Index;
        MoveNext();
        
        bool hasDecimal = false;
        while (!char.IsWhiteSpace(Current) && Current != '\0')
        {
            if (char.IsDigit(Current)) MoveNext();
            else if (Current == '.')
            {
                if (!char.IsDigit(Next)) break;
                if (!hasDecimal)
                {
                    hasDecimal = true;
                    MoveNext();
                }
                else
                {
                    MoveNext();
                    BadToken badToken = new BadToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column));
                    m_Exceptions.Add(new UnrecognizedToken(badToken));
                    return badToken;
                }
            }
            else if (char.IsLetter(Current))
            {
                MoveNext();
                BadToken badToken = new BadToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column));
                m_Exceptions.Add(new UnrecognizedToken(badToken));
                return badToken;
            }
            else break;
        }

        return hasDecimal
            ? new DoubleToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column))
            : new LongToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column));
    }
    
    private MigxnToken ReadString(int startIndex, int line, int column)
    {
        int start = m_Index;
        (int, int, int) rawPosition = (m_Index, m_LineNumber, m_ColumnNumber);
        MoveNext();
        while (Current != '\0' && Current != '"')
        {
            if (Current == '\\' && Next != '\0') MoveNext();
            MoveNext();
        }
        if (Current != '"')
        {
            (m_Index, m_LineNumber, m_ColumnNumber) = rawPosition;
            while (!char.IsWhiteSpace(Current) && Current != '\0') MoveNext();
            ReadOnlyMemory<char> text = m_Input.AsMemory()[start..m_Index];
            
            BadToken badToken = new BadToken(text, startIndex, (line, column));
            m_Exceptions.Add(new UnrecognizedToken(badToken));
            return badToken;
        }

        MoveNext();
        return new StringToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column));
    }

    private CharToken ReadChar(int startIndex, int line, int column)
    {
        int start = m_Index;
        MoveNext(); // Skip '
        if (Current == '\0' || Current == '\n')
        {
            CharToken badToken = new CharToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column));
            m_Exceptions.Add(new UnrecognizedToken(badToken));
            return badToken;
        }
        if (Current == '\\' && Next != '\0') MoveNext();
        MoveNext(); // Skip character
        CharToken charToken = new CharToken(m_Input.AsMemory()[start..m_Index], startIndex, (line, column));
        if (Current == '\'') MoveNext();
        else m_Exceptions.Add(new UnrecognizedToken(charToken));
        return charToken;
    }

    private static readonly HashSet<string> s_MultiCharOperators =
        [">=", "<=", "!=", "==", "|>", "&&", "||", "++", "--", "->", "<<", ">>"];
    
    private static bool IsOperatorChar(char c) => "+-*/%=(){}[],.<>?:|&^".Contains(c);

    private MigxnOperator ReadOperator(int startIndex, int line, int column)
    {
        int start = m_Index;
        char firstChar = MoveNext();
        if (IsOperatorChar(Current) && s_MultiCharOperators.Contains($"{firstChar}{Current}")) MoveNext();
        
        string text = m_Input[start..m_Index];
        return Operator.TryGetValue(text, out Func<int, (int, int), MigxnOperator>? tokenFactory)
            ? tokenFactory(startIndex, (line, column))
            : new MigxnOperator(text.AsMemory(), startIndex, (line, column));
    }
    
    private MigxnToken ReadIdentifier(int startIndex, int line, int column)
    {
        int start = m_Index;
        while (char.IsLetterOrDigit(Current) || Current == '_') MoveNext();
        string text = m_Input[start..m_Index];
        return Keyword.TryGetValue(text, out Func<int, (int, int), AbstractKeyword>? tokenFactory)
            ? tokenFactory(startIndex, (line, column))
            : new SymbolToken(text.AsMemory(), startIndex, (line, column));
    }

}