namespace MiHoMiao.Migxin.FrontEnd.Lexical.Operators.Calc;

internal record DotToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => ".";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new DotToken(start, position);
    public static HashSet<char> StartChars => [.."."];
    public static uint Priority => 1;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<DotToken>(migxinLexer);
    
}