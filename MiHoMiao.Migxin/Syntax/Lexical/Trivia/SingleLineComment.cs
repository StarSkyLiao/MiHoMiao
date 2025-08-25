namespace MiHoMiao.Migxin.Syntax.Lexical.Trivia;

internal record SingleLineComment(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : TriviaToken(Text, Index, Position)
{
    
}