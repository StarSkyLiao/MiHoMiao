using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Migxn.Syntax.Lexers.Exceptions;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Comments;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers;

public class MigxnLexer(string input)
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

    #region Information

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
    private readonly List<Exception> m_Exceptions = [];
    
    private char Current => m_Index >= input.Length ? '\0' : input[m_Index];

    private char Next => m_Index + 1 >= input.Length ? '\0' : input[m_Index + 1];
    
    private char Peek(int offset)
    {
        int index = m_Index + offset;
        return index >= input.Length ? '\0' : input[index];
    }
    
    private char MoveNext()
    {
        int index = m_Index++;
        if (index >= input.Length) return '\0';
        char c = input[index];
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
    
    public IEnumerable<MigxnToken> Lex()
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
                m_Exceptions.Add(new UnrecognisedTokenException((startLine, startColumn), invalid.AsMemory()));
            }
            SkipWhiteSpace();
        }
    }
    
    public IEnumerable<Exception> Exceptions => m_Exceptions;
    
    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(Current)) MoveNext();
    }

    private SingleLineComment ReadSingleLineComment(int startIndex, int line, int column)
    {
        int start = m_Index;
        while (Current != '\0' && Current != '\n') MoveNext();
        return new SingleLineComment(startIndex, input.AsMemory()[start..m_Index], (line, column));
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
        return new MultiLineComment(startIndex, input.AsMemory()[start..m_Index], (line, column));
    }
    
    private LiteralToken ReadNumber(int startIndex, int line, int column)
    {
        int start = m_Index;
        MoveNext();
        
        bool hasDecimal = false;
        while (!char.IsWhiteSpace(Current))
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
                    m_Exceptions.Add(new UnrecognisedTokenException((line, column), input.AsMemory()[start..m_Index]));
                    return new BadToken(startIndex, input.AsMemory()[start..m_Index], (line, column));
                }
            }
            else
            {
                MoveNext();
                m_Exceptions.Add(new UnrecognisedTokenException((line, column), input.AsMemory()[start..m_Index]));
                return new BadToken(startIndex, input.AsMemory()[start..m_Index], (line, column));
            }
        }

        return hasDecimal
            ? new DoubleToken(startIndex, input.AsMemory()[start..m_Index], (line, column))
            : new LongToken(startIndex, input.AsMemory()[start..m_Index], (line, column));
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
            while (!char.IsWhiteSpace(Current)) MoveNext();
            ReadOnlyMemory<char> text = input.AsMemory()[start..m_Index];
            m_Exceptions.Add(new UnrecognisedTokenException((line, column), text));
            return new BadToken(startIndex, text, (line, column));
        }

        MoveNext();
        return new StringToken(startIndex, input.AsMemory()[start..m_Index], (line, column));
    }

    private CharToken ReadChar(int startIndex, int line, int column)
    {
        int start = m_Index;
        MoveNext(); // Skip '
        if (Current == '\0' || Current == '\n')
        {
            m_Exceptions.Add(new UnrecognisedTokenException((line, column), input.AsMemory()[start..m_Index]));
            return new CharToken(startIndex, input.AsMemory()[start..m_Index], (line, column));
        }
        if (Current == '\\' && Next != '\0') MoveNext();
        MoveNext(); // Skip character
        if (Current != '\'')
        {
            m_Exceptions.Add(new UnrecognisedTokenException((line, column), input.AsMemory()[start..m_Index]));
        }
        else
        {
            MoveNext(); // Skip closing '
        }
        return new CharToken(startIndex, input.AsMemory()[start..m_Index], (line, column));
    }
    
    private static readonly HashSet<string> s_MultiCharOperators = [">=", "<=", "!=", "==", "|>", "&&", "||", "++", "--", "->"];
    
    private static bool IsOperatorChar(char c) => "+-*/=(){}[],.<>?:|&^".Contains(c);

    private AbstractOperator ReadOperator(int startIndex, int line, int column)
    {
        int start = m_Index;
        char firstChar = MoveNext();
        if (s_MultiCharOperators.Contains($"{firstChar}{Next}")) MoveNext();

        return new AbstractOperator(startIndex, input.AsMemory()[start..m_Index], (line, column));
    }
    
    private MigxnToken ReadIdentifier(int startIndex, int line, int column)
    {
        int start = m_Index;
        while (char.IsLetterOrDigit(Current) || Current == '_') MoveNext();
        string text = input[start..m_Index];
        return Keyword.TryGetValue(text, out Func<int, (int, int), AbstractKeyword>? tokenFactory)
            ? tokenFactory(startIndex, (line, column))
            : new DefaultToken(text.AsMemory(), startIndex, (line, column));
    }

}