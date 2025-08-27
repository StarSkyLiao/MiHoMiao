namespace MiHoMiao.Migxin.FrontEnd.Lexical.Trivia;

internal abstract record TriviaToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinToken(Text, Index, Position)
{

}