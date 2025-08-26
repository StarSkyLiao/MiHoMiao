using MiHoMiao.Core.Serialization.IO;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Lexical;

namespace MiHoMiao.Migxin.Syntax.Lexical.Trivia;

internal record MultiLineComment(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : TriviaToken(Text, Index, Position), ITokenMatcher
{
    public static HashSet<char> StartChars => [.."#"];
    public static uint Priority => 3;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (migxinLexer.Current is not '#') return null;
        if (migxinLexer.Peek(1) is not '#' || migxinLexer.Peek(2) is not '#') return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
        migxinLexer.MoveAhead(3);
        while (true)
        {
            if (migxinLexer.Current is '\0')
            {
                ReadOnlySpan<char> fault = migxinLexer.AsSpan(start, migxinLexer.CharIndex);
                migxinLexer.Exceptions.Add(new UnknownToken(position, fault.Escape()));
                // migxinLexer.RestoreFrame();
            }
            else if (migxinLexer.Current != '#' || migxinLexer.Peek(1) != '#' || migxinLexer.Peek(2) != '#')
            {
                migxinLexer.MoveNext();
                continue;
            }
            migxinLexer.MoveAhead(3);
            break;
        }
        ReadOnlySpan<char> read = migxinLexer.AsSpan(start, migxinLexer.CharIndex);
        return new MultiLineComment(read.Escape().AsMemory(), start, position);
    }
}