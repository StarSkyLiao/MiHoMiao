namespace MiHoMiao.Migxin.Syntax.Lexical.Trivia;

internal record TriviaToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinToken(Text, Index, Position)
{
    public override MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        throw new NotImplementedException();
    }
}