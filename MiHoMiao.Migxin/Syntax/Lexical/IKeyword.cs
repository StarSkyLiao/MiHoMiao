namespace MiHoMiao.Migxin.Syntax.Lexical;

public interface IKeyword
{
    /// <summary>
    /// 关键字对应的原始文本
    /// </summary>
    static abstract string Keyword { get; }

    /// <summary>
    /// 创建指定的 Token.
    /// </summary>
    static abstract MigxinToken LoadToken(int start, (int Line, int Column) position);
    
    public static MigxinToken? TryMatch<T>(MigxinLexer migxinLexer) where T : IKeyword
    {
        if (migxinLexer.Current != T.Keyword[0]) return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        migxinLexer.MoveAhead(T.Keyword.Length);
        ReadOnlySpan<char> read = migxinLexer.AsSpan(start, migxinLexer.CharIndex);
        if (read.SequenceEqual(T.Keyword)) return T.LoadToken(start, position);
        migxinLexer.RestoreFrame();
        return null;
    }
}