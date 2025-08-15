namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal abstract record AbstractKeyword(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)
{



}