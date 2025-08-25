namespace MiHoMiao.Migxin.Syntax.Lexical.Literals;

internal record Int64Token(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public required long Value { get; init; }

    public override MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (migxinLexer.Current is < '0' or > '9') return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        while (migxinLexer.Current is < '0' or > '9') migxinLexer.MoveNext();
        ReadOnlyMemory<char> read = migxinLexer.AsMemory(start, migxinLexer.CharIndex);
        bool success = long.TryParse(read.Span, out long result);
        if (migxinLexer.Current is 'l' or 'L') migxinLexer.MoveNext();
        if (success) return new Int64Token(read, start, position) { Value = result };
        migxinLexer.RestoreFrame();
        return null;
    }
}