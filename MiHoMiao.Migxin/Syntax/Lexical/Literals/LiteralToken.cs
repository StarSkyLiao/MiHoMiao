namespace MiHoMiao.Migxin.Syntax.Lexical.Literals;

internal abstract record LiteralToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinToken(Text, Index, Position)
{

    protected static bool IsPunctuation(char c) => "~`!@#$%^&*()-+={}[]|\\:;\"\'<>,.?/".Contains(c);
    
    protected static void ReadAheadWord(MigxinLexer migxinLexer)
    {
        char current = migxinLexer.Current;
        while (char.IsWhiteSpace(current) || IsPunctuation(current) || char.IsPunctuation(current))
        {
            migxinLexer.MoveNext();
            current = migxinLexer.Current;
        }
    }
    
}