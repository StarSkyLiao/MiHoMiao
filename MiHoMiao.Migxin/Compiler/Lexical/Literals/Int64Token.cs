namespace MiHoMiao.Migxin.Compiler.Lexical.Literals;

internal record Int64Token(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position), ITokenMatcher
{
    public required long Value { get; init; }
    public static HashSet<char> StartChars => [.."0123456789"];
    public static uint Priority => 1;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (!StartChars.Contains(migxinLexer.Current)) return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        migxinLexer.MoveNext();
        while (migxinLexer.Current is >= '0' and <= '9') migxinLexer.MoveNext();
        ReadOnlyMemory<char> read = migxinLexer.AsMemory(start, migxinLexer.CharIndex);
        bool success = long.TryParse(read.Span, out long result);
        
        if (char.IsAsciiLetter(migxinLexer.Current))
        {
            if (migxinLexer.Current is 'l' or 'L') migxinLexer.MoveNext();
            else
            {
                migxinLexer.RestoreFrame();
                return null;
            }
        }
        if (success) return new Int64Token(read, start, position) { Value = result };
        migxinLexer.RestoreFrame();
        return null;
    }
}