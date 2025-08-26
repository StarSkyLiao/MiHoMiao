namespace MiHoMiao.Migxin.Syntax.Lexical.Names;

internal record VerbatimNameToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : NameToken(Text, Index, Position), ITokenMatcher
{
    public new static HashSet<char> StartChars => [.."@"];
    public new static uint Priority => 0;
    public new static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (migxinLexer.Current is not '@') return null;
        if (!IsNameStart(migxinLexer.Next)) return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        migxinLexer.MoveAhead(2);
        while (IsNameChar(migxinLexer.Current)) migxinLexer.MoveNext();
        ReadOnlyMemory<char> read = migxinLexer.AsMemory(start + 1, migxinLexer.CharIndex);
        return new VerbatimNameToken(read, start, position);
    }
}