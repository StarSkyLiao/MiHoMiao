using MiHoMiao.Core.Serialization.IO;

namespace MiHoMiao.Migxin.FrontEnd.Lexical.Trivia;

internal record SingleLineComment(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : TriviaToken(Text, Index, Position), ITokenMatcher
{
    public static HashSet<char> StartChars => [.."#"];
    public static uint Priority => 1;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (migxinLexer.Current is not '#') return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        while (migxinLexer.Current is not '\0' and not '\n') migxinLexer.MoveNext();
        migxinLexer.MoveNext();
        ReadOnlySpan<char> read = migxinLexer.AsSpan(start, migxinLexer.CharIndex);
        return new SingleLineComment(read.Escape().AsMemory(), start, position);
    }
}