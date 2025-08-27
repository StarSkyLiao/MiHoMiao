namespace MiHoMiao.Migxin.FrontEnd.Lexical.Names;

internal record NameToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinToken(Text, Index, Position), ITokenMatcher
{
    public static HashSet<char> StartChars => [.."_"];
    public static uint Priority => 0;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (!IsNameStart(migxinLexer.Current)) return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        migxinLexer.MoveNext();
        while (IsNameChar(migxinLexer.Current)) migxinLexer.MoveNext();
        ReadOnlyMemory<char> read = migxinLexer.AsMemory(start, migxinLexer.CharIndex);
        return new NameToken(read, start, position);
    }

    protected static bool IsNameStart(char value) => value is '_' || char.IsLetter(value);
    
    protected static bool IsNameChar(char value) => value is '_' || char.IsDigit(value) || char.IsLetter(value);
    
}