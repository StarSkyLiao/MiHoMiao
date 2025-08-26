namespace MiHoMiao.Migxin.Syntax.Lexical;

internal record NameToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinToken(Text, Index, Position), ITokenMatcher
{
    public static HashSet<char> StartChars => [.."_"];
    public static uint Priority => 0;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (!StartChars.Contains(migxinLexer.Current) && !char.IsLetter(migxinLexer.Current)) return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        migxinLexer.MoveNext();
        while (StartChars.Contains(migxinLexer.Current) || char.IsLetter(migxinLexer.Current) || char.IsDigit(migxinLexer.Current)) migxinLexer.MoveNext();
        ReadOnlyMemory<char> read = migxinLexer.AsMemory(start, migxinLexer.CharIndex);
        return new NameToken(read, start, position);
    }
}