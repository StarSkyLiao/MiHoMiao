namespace MiHoMiao.Migxin.Syntax.Lexical.Operators.Calc;

internal record MulToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeyword, ITokenMatcher
{
    public static string Keyword => "*";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new MulToken(start, position);

    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeyword.TryMatch<MulToken>(migxinLexer);
    
}